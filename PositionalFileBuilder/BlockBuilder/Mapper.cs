using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using PositionalFileBuilder.BlockBuilder.Enums;
using PositionalFileBuilder.BlockBuilder.Interfaces;

namespace PositionalFileBuilder.BlockBuilder
{
    public abstract class Mapper<TEntity> : IMapperCreate<TEntity>, IMapperOptions<TEntity>, IMapperFiller<TEntity> where TEntity : class
    {
        protected List<MapObject<TEntity>> MapObjects;

        private readonly Regex _positionCheckerStartEnd;
        private readonly Regex _positionChecker;

        private readonly StringBuilder _buffer;

        protected Mapper()
        {
            MapObjects = new List<MapObject<TEntity>>();
            _positionCheckerStartEnd = new Regex("^[0-9]{1,5}-[0-9]{1,5}$", RegexOptions.Compiled);
            _positionChecker = new Regex("^[0-9]{1,5}$", RegexOptions.Compiled);

            _buffer = new StringBuilder();
        }

        public string Serialize(TEntity pocmex)
        {
            var stb = GetBuilder(pocmex);

            return stb.ToString();
        }

        public void SerializeToBuffer(TEntity pocmex)
        {
            var stb = GetBuilder(pocmex);

            _buffer.Append(stb);
        }

        private StringBuilder GetBuilder(TEntity pocmex)
        {
            var stb = new StringBuilder();

            MapObjects.OrderBy(o => o.PositionStart).ToList().ForEach(item =>
            {
                var value = item.Compiled?.Invoke(pocmex).ToString() ?? string.Empty;
                if (value.Length > item.Size) throw new ArgumentException($"Size of value is bigger than size of position. Informed: {value.Length}. Expected: {item.Size}. Diff: {value.Length - item.Size}.");
                stb.Append(item.Padding == JustifiedEnum.RightJustified
                    ? value.PadLeft(item.Size, item.Filler)
                    : value.PadRight(item.Size, item.Filler));
            });

            if (IncludeNewLine) stb.Append(Environment.NewLine);

            return stb;
        }

        public string GetBuffer()
        {
            return _buffer.ToString();
        }

        public int TotalSize { get; set; }

        public bool IncludeNewLine { get; set; }

        #region Interfaces

        public IMapperOptions<TEntity> Map(Expression<Func<TEntity, object>> toMap)
        {
            if (MapObjects.Any(a => a.ToMap?.Body.ToString() == toMap.Body.ToString())) throw new Exception("Attribute already mapped.");
            MapObjects.Add(new MapObject<TEntity>(toMap));
            return this;
        }

        public IMapperFiller<TEntity> MapFiller()
        {
            MapObjects.Add(new MapObject<TEntity>());
            return this;
        }

        IMapperFiller<TEntity> IMapperFiller<TEntity>.Filler(char character)
        {
            SetFiller(character);
            return this;
        }

        public IMapperOptions<TEntity> Filler(char character)
        {
            SetFiller(character);
            return this;
        }

        private void SetFiller(char character)
        {
            MapObjects.Last().Filler = character;
        }

        public IMapperOptions<TEntity> Position(string position)
        {
            return SetPosition(position);
        }

        IMapperFiller<TEntity> IMapperFiller<TEntity>.Position(string position)
        {
            return SetPosition(position);
        }

        public IMapperOptions<TEntity> Padding(JustifiedEnum padding)
        {
            MapObjects.Last().Padding = padding;
            return this;
        }

        public IMapperOptions<TEntity> Size(int size)
        {
            MapObjects.Last().Size = size;
            return this;
        }

        #endregion

        private Mapper<TEntity> SetPosition(string position)
        {
            var item = MapObjects.Last();

            if (_positionChecker.Match(position).Success)
            {
                item.Position = position;
                int.TryParse(position, out var value);

                item.Size = +1;
                item.PositionStart = value;
                item.PositionEnd = value;

                return this;
            }

            if (!_positionCheckerStartEnd.Match(position).Success)
                throw new Exception($"Format Position is invalid. Valid format: 999-999. Check: {ExpressionName(item.ToMap)}");

            item.Position = position;
            var numbers = position.Split('-').Select(int.Parse).ToList();
            var size = numbers[1] - numbers[0];

            if (size <= 0) throw new Exception("Size cannot be equals 0 or negative. Check position values.");

            item.Size = size + 1;
            item.PositionStart = numbers[0];
            item.PositionEnd = numbers[1];

            return this;
        }

        protected void MapCheck()
        {
            var currentSize = MapObjects.Sum(s => s.Size);
            if (TotalSize != currentSize)
                throw new Exception($"Total size mismatch. Informed: {TotalSize}. Generated: {currentSize}");

            var itens = MapObjects.OrderBy(o => o.PositionStart).ToList();

            if (itens.First().PositionStart != 1)
                throw new Exception("A Map position starting at '1' does not exist.");

            for (var i = 0; i < itens.Count - 1; i++)
            {
                if (itens[i + 1].PositionStart - itens[i].PositionEnd != 1)
                    throw new Exception($"Position mapped not valid (Gap found). Check: {ExpressionName(itens[i + 1].ToMap)}");
            }
        }

        private static string ExpressionName(Expression<Func<TEntity, object>> item)
        {
            if (item == null) return "Filler";

            switch (item.Body)
            {
                case MemberExpression _:
                    return ((MemberExpression)item.Body).Member.Name;

                case UnaryExpression _:
                    return ((MemberExpression)((UnaryExpression)item.Body).Operand).Member.Name;

                default:
                    return item.Name;
            }
        }
    }
}
