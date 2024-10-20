using SharedLibrary.Responses;
using System.Linq.Expressions;

namespace SharedLibrary
{
    public interface IGenericInterface<T> where T : class
    {
        Task<ServiceResponse> CreateAsync (T entity);
        Task<ServiceResponse> UpdateAsync (T entity);
        Task<ServiceResponse> DeleteAsync (T entity);
        Task<IEnumerable<T>> GetAllAsync ();
        Task<T> GetByIdAsync (int id);
        Task<T> GetByAsync (Expression<Func<T, bool>> predicate);
    }
}
