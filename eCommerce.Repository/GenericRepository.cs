using eCommerce.Core.Entities;
using eCommerce.Core.Repositories;
using eCommerce.Core.Specifications;
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
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationsAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecificationsAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task<int> GetCountWithSpecification(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task AddAsync(T item)
        => await _dbContext.Set<T>().AddAsync(item);


        public void Delete(T item)
        => _dbContext.Set<T>().Remove(item);

        public void Update(T item)
        => _dbContext.Set<T>().Update(item);
    }
}