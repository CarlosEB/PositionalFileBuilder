using PositionalFileBuilder.BlockBuilder.Enums;

namespace PositionalFileBuilder.BlockBuilder.Interfaces
{
    public interface IMapperOptions<TEntity> where TEntity : class
    {
        IMapperOptions<TEntity> Padding(JustifiedEnum padding);

        IMapperOptions<TEntity> Size(int size);

        IMapperOptions<TEntity> Filler(char character);

        IMapperOptions<TEntity> Position(string position);
    }
}
