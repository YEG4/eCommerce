using eCommerce.Core.Entities;
using eCommerce.Core.Repositories;
using eCommerce.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        public StoreContext _dbContext { get; }
        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
    }
}