﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaRepository.GenericRepository
{
	public interface IGenericRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
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
