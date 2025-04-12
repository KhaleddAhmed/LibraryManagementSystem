using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Repository.Contract;
using LibraryManagementSystem.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository.Repository
{
    public class GenericRepository<T, Tkey> : IGenericRepository<T, Tkey>
        where T : class
    {
        private readonly LibraryDbContext _libraryDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
            _dbSet = _libraryDbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _libraryDbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(Tkey id)
        {
            return await _libraryDbContext.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _libraryDbContext.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _libraryDbContext.Update(entity);
        }

        public void Delete(T entity)
        {
            _libraryDbContext.Remove(entity);
        }

        public async Task<IQueryable<T>> GetAllAsyncAsQueryable() => _dbSet.AsNoTracking();

        public async Task<IQueryable<T>> Get(Expression<Func<T, bool>> predict = null)
        {
            return _dbSet.Where(predict);
        }
    }
}
