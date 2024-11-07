using eCommerce.Core.Entities;
using eCommerce.Core.Specifications;

namespace eCommerce.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);

        public Task<IReadOnlyList<T>> GetAllWithSpecificationsAsync(ISpecifications<T> spec);
        public Task<T> GetByIdWithSpecificationsAsync(ISpecifications<T> spec);
        public Task<int> GetCountWithSpecification(ISpecifications<T> spec);
    }
}