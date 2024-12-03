using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Core;
using eCommerce.Core.Entities;
using eCommerce.Core.Repositories;
using eCommerce.Repository.Data;

namespace eCommerce.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(StoreContext db)
        {
            _repositories = new Hashtable();
            this._dbContext = db;
        }
        public Task<int> CompleteAsync()
        => _dbContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
        => _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, repository);
            }
            return (IGenericRepository<TEntity>) _repositories[type];
        }
    }
}
