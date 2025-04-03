using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MilkTea.Repository.Model;
using MilkTeaRepository.GenericRepository;

namespace MilkTeaRepository.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ThreeBrothersMilkTeaShopContext _context;
		private bool disposed = false;
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

		public void BeginTransaction()
		{
			_context.Database.BeginTransaction();
		}

		public void CommitTransaction()
		{
			_context.Database.CommitTransaction();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			disposed = true;
		}

		public void RollbackTransaction()
		{
			_context.Database.RollbackTransaction();
		}
	}
}
