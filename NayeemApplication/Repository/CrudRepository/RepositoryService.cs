using NayeemApplication.Data;
using NayeemApplication.Data.Entity;
using NayeemApplication.Repository.CrudRepository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace NayeemApplication.Repository.CrudRepository
{
    public class RepositoryService<T> : IRepositoryService<T> where T : Base
    {

        #region Constructor Section
        protected AppDbContext _context;
        public RepositoryService(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAll Section
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public IQueryable<T> FindAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = FindAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty).AsNoTracking();
            }

            return queryable;
        }
        #endregion

        #region GetSingleById Section

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetAsync(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        #endregion

        #region GetSingleleWithFilter Section
        public T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(match);
        }
        public T FindFirst(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().OrderByDescending(match).AsNoTracking().FirstOrDefault(match);
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(match);
        }

        #endregion

        #region GetMultipleWithFilter Section
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).AsNoTracking().ToList();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
                .Where(expression).AsNoTracking();
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).AsNoTracking().ToListAsync();
        }

        public IEnumerable<T> FindListBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _context.Set<T>().Where(predicate).AsNoTracking();
            return query;
        }

        public async Task<IEnumerable<T>> FindListByAsyn(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        public IQueryable<T> GetWithIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = GetAll().AsQueryable();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public async Task<IQueryable<T>> GetAllWithIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = (IQueryable<T>)await GetAllAsync();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public IQueryable<T> GetAllFilterWithRelational(Expression<Func<T, bool>> predicate, string includeTable = "")
        {
            IQueryable<T> result = _context.Set<T>().Where(predicate);
            if (includeTable != "")
                result = result.Include(includeTable);

            return result;
        }
        #endregion

        #region Save Single Section
        public int Add(T Instance)
        {
            _context.Set<T>().Add(Instance);
            return _context.SaveChanges();

        }

        public async Task<int> AddAsync(T Instance)
        {
            _context.Set<T>().Add(Instance);
            return await _context.SaveChangesAsync();

        }
        #endregion

        #region Save Range Section
        public async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Single Section
        public string Update(T updated, string key)
        {
            if (updated == null)
                return string.Empty;

            T existing = _context.Set<T>().Find(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updated);
                _context.SaveChanges();
            }
           return key;
        }

        public async Task<string> UpdateAsync(T updated, string key)
        {
            if (updated == null)
                return string.Empty;

            T existing = await _context.Set<T>().FindAsync(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updated);
                await _context.SaveChangesAsync();
            }
            return key;
        }
        #endregion

        #region Delete Single Section

        public bool Delete(T Instance)
        {
            _context.Set<T>().Remove(Instance);
            return 1 == _context.SaveChanges();
        }

        public async Task<bool> DeleteAsync(T Instance)
        {
            _context.Set<T>().Remove(Instance);
            return 1 == await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete Range Section

        public async Task<bool> DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion

        #region Count Section
        public int Count()
        {
            return _context.Set<T>().AsNoTracking().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().AsNoTracking().CountAsync();
        }
        #endregion

        #region ExistCheck Section

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsNoTracking().AsQueryable().Any(predicate);
        }
        #endregion

        #region Dispose Section
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
