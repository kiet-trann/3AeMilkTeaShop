using AutoMapper;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTeaRepository.UnitOfWork;

namespace MilkTea.Services.OrderServices
{
	public class OrderService : IOrderService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<OrderViewModel> GetOrderByIdAsync(int orderId)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var order = await _unitOfWork.GetRepository<Order>()
					.GetFirstOrDefaultAsync(o => o.OrderId == orderId, includeProperties: "User");

				if (order == null)
				{
					_unitOfWork.CommitTransaction();
					return null;
				}

				_unitOfWork.CommitTransaction();
				return _mapper.Map<OrderViewModel>(order);
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public async Task<List<OrderDetailViewModel>> GetOrderDetailsForOrder(int orderId)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var orderDetails = await _unitOfWork.GetRepository<OrderDetail>()
					.GetPaginateAsync(1, int.MaxValue, od => od.OrderId == orderId, includeProperties: "Product");

				_unitOfWork.CommitTransaction();
				return _mapper.Map<List<OrderDetailViewModel>>(orderDetails);
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public async Task<IEnumerable<OrderViewModel>> GetPaginatedOrdersAsync(int pageIndex, int pageSize)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var orders = await _unitOfWork.GetRepository<Order>().GetPaginateAsync(pageIndex, pageSize, includeProperties: "User");
				_unitOfWork.CommitTransaction();
				return _mapper.Map<IEnumerable<OrderViewModel>>(orders);
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public async Task<string> UpdateOrderStatusAsync(int orderId, string newStatus)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var order = await _unitOfWork.GetRepository<Order>().GetByIdAsync(orderId);
				if (order == null)
				{
					_unitOfWork.CommitTransaction();
					return "Đơn hàng không tồn tại.";
				}

				order.Status = newStatus;
				_unitOfWork.GetRepository<Order>().Update(order);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
				return "Cập nhật trạng thái thành công.";
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				return "Lỗi khi cập nhật trạng thái.";
			}
		}
	}
}
