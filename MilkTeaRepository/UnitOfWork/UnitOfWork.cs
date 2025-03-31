using MilkTeaRepository.GenericRepository;
using MilkTeaRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaRepository.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly _3anhEmMilkTeaShopContext _context;

		public UnitOfWork(_3anhEmMilkTeaShopContext context)
		{
			_context = context;
			ProductRepository = new GenericRepository<Product>(_context);
			OrderRepository = new GenericRepository<Order>(_context);
			// Khởi tạo các repositories khác
		}

		public IGenericRepository<Product> ProductRepository { get; }
		public IGenericRepository<Order> OrderRepository { get; }

		public async Task<int> CommitAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
