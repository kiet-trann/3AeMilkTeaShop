using Microsoft.EntityFrameworkCore;
using MilkTeaRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaRepository.GenericRepository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly _3anhEmMilkTeaShopContext _context;
		protected readonly DbSet<T> _dbSet;

		public GenericRepository(_3anhEmMilkTeaShopContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync(
			Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
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

			if (orderBy != null)
			{
				return await orderBy(query).ToListAsync();
			}
			else
			{
				return await query.ToListAsync();
			}
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
			_dbSet.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
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
	}
}
