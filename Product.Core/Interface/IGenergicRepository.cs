using Product.Core.Entities;
using System.Linq.Expressions;

namespace Product.Core.Interface
{
    public interface IGenergicRepository<T> where T : BasicEntity<int>
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        Task<T> GetByidAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<T> GetAsync(int id);
        Task AddAsync(T Entity);
        Task DeleteAsync(int id);

        Task UpdateAsync(int id, T Entity);
    }
}
