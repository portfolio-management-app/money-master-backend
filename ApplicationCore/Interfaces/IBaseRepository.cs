using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.Entity;
using Microsoft.EntityFrameworkCore.Query;

namespace ApplicationCore.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        int Count(Expression<Func<TEntity, bool>> filter = null);
        void Delete(TEntity entity);

        TEntity Insert(TEntity entity);

        void InsertRange(List<TEntity> entities);

        TEntity Update(TEntity entity);

        IEnumerable<TEntity> ListAll();

        IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null
        );

        decimal CalculateSum(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
    }
}