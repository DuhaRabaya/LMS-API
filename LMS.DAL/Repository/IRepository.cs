using LMS.DAL.Models;

namespace LMS.DAL.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(Object id);
        Task<T> Add(T entity);
        Task Remove(T entity);
        IQueryable<T> Query();
        Task Update(T entity);
    }
}