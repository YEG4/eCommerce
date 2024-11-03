using eCommerce.Core.Entities;

namespace eCommerce.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
    }
}