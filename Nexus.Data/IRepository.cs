using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nexus.Core;

namespace Nexus.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        DbContext UnitOfWork { get; }

        TEntity Get(int id);
        Task<TEntity> GetAsync(int id);

        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();

        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> expression);

        IPagedList<TEntity> GetPaged<TS>(Expression<Func<TEntity, TS>> orderByExpression, int page, int pageSize);
        IPagedList<TEntity> GetPagedDescending<TS>(Expression<Func<TEntity, TS>> orderByExpression, int page, int pageSize);

        IEnumerable<TEntity> AllInclude(params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> AllIncludeFiltered(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        void Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void Update(IEnumerable<TEntity> entities);

        void Delete(int id);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
    }
}