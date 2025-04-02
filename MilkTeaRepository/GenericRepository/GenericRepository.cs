﻿using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
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

		public async Task UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
			await Task.CompletedTask;
		}

		public async Task RemoveAsync(object id)
		{
			T entity = await _dbSet.FindAsync(id);
			await RemoveAsync(entity);
		}

		public async Task RemoveAsync(T entity)
		{
			if (_context.Entry(entity).State == EntityState.Detached)
			{
				_dbSet.Attach(entity);
			}
			_dbSet.Remove(entity);
			await Task.CompletedTask;
		}

		public async Task RemoveRangeAsync(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
			await Task.CompletedTask;
		}

		public async Task<int> CountAsync()
		{
			return await _dbSet.CountAsync();
		}
	}
}
