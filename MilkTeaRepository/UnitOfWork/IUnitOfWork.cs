using MilkTeaRepository.GenericRepository;

namespace MilkTeaRepository.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<T> GetRepository<T>() where T : class;
		Task<int> SaveChangesAsync();
		void BeginTransaction();
		void CommitTransaction();
		void RollbackTransaction();
	}
}
