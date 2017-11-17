using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PositionalFileBuilder.BlockBuilder.Enums;

namespace PositionalFileBuilder.BlockBuilder
{
    public class MapObject<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, object>> ToMap { get; set; }
        public Func<TEntity, object> Compiled { get; set; }

        public string Position { get; set; }

        public int PositionStart { get; set; }

        public int PositionEnd { get; set; }

        public int Size { get; set; }

        public char Filler { get; set; }

        public char FillerNumeric { get; set; } = '0';

        public char FillerString { get; set; } = ' ';

        public JustifiedEnum Padding { get; set; }

        public MapObject()
        {
            ToMap = null;
            Padding = JustifiedEnum.RightJustified;
            Filler = ' ';
            Compiled = null;
        }

        public MapObject(Expression<Func<TEntity, object>> toMap)
        {
            var isnumeric = toMap.Body is UnaryExpression unary && IsNumericType(unary.Operand.Type);

            ToMap = toMap;
            Padding = !isnumeric ? JustifiedEnum.LeftJustified : JustifiedEnum.RightJustified;
            Filler = isnumeric ? FillerNumeric : FillerString;
            Compiled = toMap.Compile();
        }

        public bool IsNumericType(Type type)
        {
            return _numericTypes.Contains(type) || _numericTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        private readonly HashSet<Type> _numericTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(double),
            typeof(decimal)
        };
    }
}
