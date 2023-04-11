using PaymentSimple.Core.Domain;
using PaymentSimple.Core.Domain.Models;

namespace PaymentSimple.Core.Abstractions.Repositories
{    
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<Card> GetCardByNumberAsync(string number);
    }
}
