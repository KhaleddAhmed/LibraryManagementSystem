using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core;
using LibraryManagementSystem.Core.Repository.Contract;
using LibraryManagementSystem.Repository.Data.Contexts;
using LibraryManagementSystem.Repository.Repository;

namespace LibraryManagementSystem.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(LibraryDbContext dbContext) // ASK CLR For Creating ObjectFrom DbContext Implicitly
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>()
            where TEntity : class
        {
            var Key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(Key))
            {
                var repository = new GenericRepository<TEntity, TKey>(_dbContext);
                _repositories.Add(Key, repository);
            }

            return _repositories[Key] as IGenericRepository<TEntity, TKey>;
        }
    }
}
