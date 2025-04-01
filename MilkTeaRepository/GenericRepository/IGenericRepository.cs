using System.Linq.Expressions;

namespace MilkTeaRepository.GenericRepository
{
	public interface IGenericRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetPaginateAsync(int pageNumber = 1,
													 int pageSize = 10,
													 Expression<Func<T, bool>>? filter = null,
													 Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
													 string includeProperties = "");
		Task<T> GetByIdAsync(object id);
		Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null,
			string includeProperties = "");
		Task AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task RemoveAsync(object id);
		Task RemoveAsync(T entity);
		Task RemoveRangeAsync(IEnumerable<T> entities);
	}
}
