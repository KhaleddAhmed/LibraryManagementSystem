using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Core.Repository.Contract;

namespace LibraryManagementSystem.Core
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();

        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>()
            where TEntity : class;
    }
}
