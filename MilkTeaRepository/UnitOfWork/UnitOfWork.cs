using Microsoft.EntityFrameworkCore.Storage;
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
		private IDbContextTransaction _transaction;
		private bool _disposed = false;

		public IGenericRepository<User> Users { get; private set; }
		public IGenericRepository<Customer> Customers { get; private set; }
		public IGenericRepository<Order> Orders { get; private set; }
		public IGenericRepository<OrderDetail> OrderDetails { get; private set; }
		public IGenericRepository<OrderDetailTopping> OrderDetailToppings { get; private set; }
		public IGenericRepository<Product> Products { get; private set; }
		public IGenericRepository<Topping> Toppings { get; private set; }
		public IGenericRepository<Category> Categories { get; private set; }

		public UnitOfWork(_3anhEmMilkTeaShopContext context)
		{
			_context = context;

			// Initialize repositories
			Users = new GenericRepository<User>(_context);
			Customers = new GenericRepository<Customer>(_context);
			Orders = new GenericRepository<Order>(_context);
			OrderDetails = new GenericRepository<OrderDetail>(_context);
			OrderDetailToppings = new GenericRepository<OrderDetailTopping>(_context);
			Products = new GenericRepository<Product>(_context);
			Toppings = new GenericRepository<Topping>(_context);
			Categories = new GenericRepository<Category>(_context);
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public async Task BeginTransactionAsync()
		{
			_transaction = await _context.Database.BeginTransactionAsync();
		}

		public async Task CommitTransactionAsync()
		{
			try
			{
				await _transaction.CommitAsync();
			}
			catch
			{
				await _transaction.RollbackAsync();
				throw;
			}
			finally
			{
				_transaction.Dispose();
				_transaction = null;
			}
		}

		public async Task RollbackTransactionAsync()
		{
			await _transaction.RollbackAsync();
			_transaction.Dispose();
			_transaction = null;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					if (_transaction != null)
					{
						_transaction.Dispose();
						_transaction = null;
					}
					_context.Dispose();
				}
				_disposed = true;
			}
		}
	}
}
