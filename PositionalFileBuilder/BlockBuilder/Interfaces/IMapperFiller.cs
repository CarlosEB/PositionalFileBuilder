namespace PositionalFileBuilder.BlockBuilder.Interfaces
{
    public interface IMapperFiller<TEntity> where TEntity : class
    {
        IMapperFiller<TEntity> Filler(char character);

        IMapperFiller<TEntity> Position(string position);

    }
}
