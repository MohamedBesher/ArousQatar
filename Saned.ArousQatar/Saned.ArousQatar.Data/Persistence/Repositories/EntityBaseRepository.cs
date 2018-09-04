using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{


    public abstract class EntityBaseRepository<T> : BaseRepositroy, IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private ApplicationDbContext _dbContext;

        #region Properties

        public IDbFactory DbFactory { get; private set; }

        public ApplicationDbContext DbContext => _dbContext ?? (_dbContext = DbFactory.Init());


        protected EntityBaseRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        #endregion


        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query.Include(includeProperty);
            }
            return query;
        }

        public async Task<List<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = DbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query.Include(includeProperty);
            }
            var res = query.ToListAsync();
            return await res;
        }

        public IQueryable<T> All => GetAll();

        public IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(string modelName, int start = 0, int number = 10, string filter = "")
        {
            filter = filter.Trim();
            var res = (await SqlQueryAsync("EXEC " + modelName + "GetAll @index, @rowNumber, @filter",
                 new SqlParameter("index", SqlDbType.Int) { Value = start },
                 new SqlParameter("rowNumber", SqlDbType.Int) { Value = number },
                 new SqlParameter("filter", SqlDbType.NVarChar) { Value = filter }))
                 .ToList();
            return res;
        }
        public async Task<List<K>> GetAllAsync<K>(string modelName, int start = 0, int number = 10, string filter = "")
        {
            filter = filter.Trim();
            var startPar = new SqlParameter("index", SqlDbType.Int) { Value = start };
            var numberPar = new SqlParameter("rowNumber", SqlDbType.Int) { Value = number };
            var filterPar = new SqlParameter("filter", SqlDbType.NVarChar) { Value = filter };

            var res = (await DbContext.Database.SqlQuery<K>( modelName + "GetAll @index, @rowNumber, @filter",
                                startPar, numberPar, filterPar).ToListAsync());

            return res;
        }

        public T GetSingle(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public async Task<T> GetSingleAsync(string SPName, int id)
        {
            T res = (await SqlQueryAsync("EXEC " + SPName + "GetSingle @id", new SqlParameter("id", SqlDbType.Int) { Value = id })).FirstOrDefault();
            return res;
        }

        public async Task<K> GetSingleAsync<K>(string SPName, int id)
        {
            K res =
                (await
                    DbContext.Database.SqlQuery<K>("EXEC " + SPName + "GetSingle @id",
                        new SqlParameter("id", SqlDbType.Int) { Value = id }).FirstOrDefaultAsync());
            return res;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate);
        }

        public async Task<List<T>> FindByAsnc(Expression<Func<T, bool>> predicate)
        {
            return await DbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            DbContext.Set<T>().Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            DbContext.Set<T>().Add(entity);
           
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }




        public async Task<int> DeleteAsync(T t)
        {
            DbContext.Set<T>().Remove(t);
            return await DbContext.SaveChangesAsync();
        }

        public void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }


        public async Task<T> EditAsync(T updated, int id)
        {
            if (updated == null)
                return null;

            T existing = await DbContext.Set<T>().FindAsync(id);
            if (existing != null)
            {
                DbContext.Entry(existing).CurrentValues.SetValues(updated);
                await DbContext.SaveChangesAsync();
            }
            return existing;
        }

        public DbRawSqlQuery<T> SqlQuery(string sql, params object[] parameters)
        {
            return DbContext.Database.SqlQuery<T>(sql, parameters);
        }

        public async Task<IList<T>> SqlQueryAsync(string sql, params object[] parameters)
        {
            return await (DbContext.Database.SqlQuery<T>(sql, parameters).ToListAsync());
        }

        public int Count()
        {
            return DbContext.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await DbContext.Set<T>().CountAsync();
        }
    }
}
