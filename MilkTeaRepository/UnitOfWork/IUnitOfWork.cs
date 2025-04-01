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
		IGenericRepository<User> Users { get; }
		IGenericRepository<Customer> Customers { get; }
		IGenericRepository<Order> Orders { get; }
		IGenericRepository<OrderDetail> OrderDetails { get; }
		IGenericRepository<OrderDetailTopping> OrderDetailToppings { get; }
		IGenericRepository<Product> Products { get; }
		IGenericRepository<Topping> Toppings { get; }
		IGenericRepository<Category> Categories { get; }

		Task<int> SaveChangesAsync();
		Task BeginTransactionAsync();
		Task CommitTransactionAsync();
		Task RollbackTransactionAsync();
	}
}
