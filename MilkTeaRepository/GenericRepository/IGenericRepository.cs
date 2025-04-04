using System.Linq.Expressions;

namespace MilkTeaRepository.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<IEnumerable<T>> GetPaginateAsync(int pageNumber = 1,
                                                     int pageSize = 10,
                                                     Expression<Func<T, bool>>? filter = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                     string includeProperties = "");
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> GetByIdAsync(object id);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null,
            string includeProperties = "");
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(object id);
        int Count();
    }
}
