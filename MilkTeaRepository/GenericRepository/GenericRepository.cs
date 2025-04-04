using Microsoft.EntityFrameworkCore;
using MilkTea.Repository.Model;
using System.Linq.Expressions;

namespace MilkTeaRepository.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ThreeBrothersMilkTeaShopContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ThreeBrothersMilkTeaShopContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
		public IQueryable<T> Entities => _dbSet;

		public async Task<IEnumerable<T>> GetPaginateAsync(int pageNumber,
                                                         int pageSize,
                                                         Expression<Func<T, bool>>? filter = null,
                                                         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                         string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            // Áp dụng filter nếu có
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Áp dụng các thuộc tính cần Include nếu có
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(
                    new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            // Áp dụng sắp xếp nếu có
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        // Lấy tất cả theo filter
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToList();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(object id)
        {
            T entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
        }

        public int Count()
        {
            return _dbSet.Count();
        }
    }
}
