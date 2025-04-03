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
			var order = await _unitOfWork.GetRepository<Order>()
				.GetFirstOrDefaultAsync(o => o.OrderId == orderId, includeProperties: "User");

			if (order == null)
			{
				return null;
			}

			return _mapper.Map<OrderViewModel>(order);
		}


		public async Task<List<OrderDetailViewModel>> GetOrderDetailsForOrder(int orderId)
		{
			var orderDetails = await _unitOfWork.GetRepository<OrderDetail>()
				.GetPaginateAsync(1, int.MaxValue, od => od.OrderId == orderId, includeProperties: "Product");

			return _mapper.Map<List<OrderDetailViewModel>>(orderDetails);
		}

		public async Task<IEnumerable<OrderViewModel>> GetPaginatedOrdersAsync(int pageIndex, int pageSize)
		{
			var orders = await _unitOfWork.GetRepository<Order>().GetPaginateAsync(pageIndex, pageSize, includeProperties: "User");
			return _mapper.Map<IEnumerable<OrderViewModel>>(orders);
		}
		public async Task<string> UpdateOrderStatusAsync(int orderId, string newStatus)
		{
			var order = await _unitOfWork.GetRepository<Order>().GetByIdAsync(orderId);
			if (order == null)
				return "Đơn hàng không tồn tại.";

			order.Status = newStatus;
			_unitOfWork.GetRepository<Order>().Update(order);
			await _unitOfWork.SaveChangesAsync();

			return "Cập nhật trạng thái thành công.";
		}
	}
}
