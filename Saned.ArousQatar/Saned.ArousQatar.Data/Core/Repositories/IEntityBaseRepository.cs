using Saned.ArousQatar.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<List<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> All { get; }

        Task<List<T>> GetAllAsync(string SPName, int start = 0, int number = 4, string filter = "");
        Task<List<K>> GetAllAsync<K>(string modelName, int start = 0, int number = 10, string filter = "");
        IQueryable<T> GetAll();

        T GetSingle(int id);
        Task<T> GetSingleAsync(string SPName, int id);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<List<T>> FindByAsnc(Expression<Func<T, bool>> predicate);

        void Add(T entity);
        Task<T> AddAsync(T entity);


        void Delete(T entity);
        Task<int> DeleteAsync(T entity);

        void Edit(T entity);
        Task<T> EditAsync(T entity, int id);

        int Count();
        Task<int> CountAsync();

        DbRawSqlQuery<T> SqlQuery(string sql, params object[] parameters);
        Task<IList<T>> SqlQueryAsync(string sql, params object[] parameters);
    }
}
