using Microsoft.EntityFrameworkCore;
using MilkTeaRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaRepository.GenericRepository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly _3anhEmMilkTeaShopContext _context;

		public GenericRepository(_3anhEmMilkTeaShopContext context)
		{
			_context = context;
		}

		public async Task AddAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public void Update(T entity)
		{
			_context.Set<T>().Update(entity);
		}

		public void Delete(T entity)
		{
			_context.Set<T>().Remove(entity);
		}

		public async Task<bool> Exists(int id)
		{
			return await _context.Set<T>().FindAsync(id) != null;
		}
	}
}
