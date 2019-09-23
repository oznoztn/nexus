using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nexus.Core;
using Nexus.Core.Entities;

namespace Nexus.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected DbContext Context;
        protected DbSet<TEntity> Set;

        public Repository(DbContext context)
        {
            Context = context;
            Set = context.Set<TEntity>();
        }

        /// <summary>
        /// Mocking framework only
        /// </summary>
        public Repository()
        {
            
        }

        public virtual DbContext UnitOfWork => Context;

        public virtual TEntity Get(int id)
        {            
            return Set.AsNoTracking().SingleOrDefault(e => e.Id == id);
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await Set.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Set.AsNoTracking().AsEnumerable();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Set.AsNoTracking().ToListAsync();
        }

        public virtual IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> expression)
        {
            return Set.AsNoTracking().Where(expression).AsEnumerable();
        }

        public virtual async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Set.AsNoTracking().Where(expression).ToListAsync();            
        }

        public virtual IEnumerable<TEntity> AllInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetAllIncluding(includeProperties).AsEnumerable();
        }

        public virtual IEnumerable<TEntity> AllIncludeFiltered(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = Set.AsNoTracking();
            return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Where(predicate).Include(includeProperty));
        }

        public virtual IPagedList<TEntity> GetPaged<TS>(Expression<Func<TEntity, TS>> orderByExpression, int page, int pageSize)
        {
            return new PagedList<TEntity>(Set.OrderBy(orderByExpression), page, pageSize);
        }

        // todo: UP
        public virtual IPagedList<TEntity> GetPagedDescending<TS>(Expression<Func<TEntity, TS>> orderByExpression, int page, int pageSize)
        {
            return new PagedList<TEntity>(Set.OrderByDescending(orderByExpression), page, pageSize);
        }

        private IQueryable<TEntity> GetAllIncluding (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = Set.AsNoTracking();
            return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        }
        public virtual void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Context.Add(entity);
            }
        }

        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Delete(int id)
        {
            TEntity entity = Set.SingleOrDefault(t => t.Id == id);

            if (entity != null)
            {
                Set.Remove(entity);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            //_context.Entry(entity).State = EntityState.Deleted;
            Context.Remove(entity);
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
        }
    }
}