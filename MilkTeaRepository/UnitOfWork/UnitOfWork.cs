using Microsoft.EntityFrameworkCore.Storage;
using MilkTea.Repository.Model;
using MilkTeaRepository.GenericRepository;

namespace MilkTeaRepository.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ThreeBrothersMilkTeaShopContext _context;
		private IDbContextTransaction _transaction;
		private bool _disposed = false;
		private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

		public UnitOfWork(ThreeBrothersMilkTeaShopContext context)
		{
			_context = context;
		}

		public IGenericRepository<T> GetRepository<T>() where T : class
		{
			var type = typeof(T);

			if (_repositories.ContainsKey(type))
			{
				return (IGenericRepository<T>)_repositories[type];
			}

			var repository = new GenericRepository<T>(_context);
			_repositories[type] = repository;
			return repository;
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
