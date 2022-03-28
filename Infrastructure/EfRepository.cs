using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.Entity;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure
{
    public class EfRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _dbSet;

        public EfRepository(AppDbContext dbSet)
        {
            _dbSet = dbSet;
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (include is null) return _dbSet.Set<TEntity>().FirstOrDefault(predicate);

            var queryable = include(_dbSet.Set<TEntity>());

            return queryable.FirstOrDefault(predicate);
        }


        public void Delete(TEntity entity)
        {
            _ = _dbSet.Set<TEntity>().Remove(entity);

            _dbSet.SaveChanges();
        }

        public TEntity Insert(TEntity entity)
        {
            _ = _dbSet.Add(entity);
            _dbSet.SaveChanges();
            return entity;
        }

        public void InsertRange(List<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            _dbSet.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            _dbSet.Set<TEntity>().Attach(entity);
            _dbSet.Entry(entity).State = EntityState.Modified;

            _dbSet.SaveChanges();

            return entity;
        }

        public IEnumerable<TEntity> ListAll()
        {
            return _dbSet.Set<TEntity>();
        }


        public IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = _dbSet.Set<TEntity>();
            if (filter is not null) query = query.Where(filter);

            if (include is not null) query = include(query);
            if (orderBy is not null) query = orderBy(query);

            return query.ToList();
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet.Set<TEntity>();
            if (filter is not null)
                query = query.Where(filter);

            return query.Count();
        }
    }
}