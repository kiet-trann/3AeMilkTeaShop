using MilkTeaRepository.GenericRepository;
using MilkTeaRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaRepository.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		IGenericRepository<Product> ProductRepository { get; }
		IGenericRepository<Order> OrderRepository { get; }
		// Thêm các repositories khác

		Task<int> CommitAsync();
	}
}
