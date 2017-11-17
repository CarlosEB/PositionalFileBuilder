using System;
using System.Linq.Expressions;

namespace PositionalFileBuilder.BlockBuilder.Interfaces
{
    public interface IMapperCreate<TEntity> where TEntity : class
    {
        IMapperOptions<TEntity> Map(Expression<Func<TEntity, object>> toMap);
    }
}
