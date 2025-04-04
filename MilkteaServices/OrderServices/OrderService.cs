using AutoMapper;
using MilkTea.Core.Enum;
using MilkTea.Core.Pagination;
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

		public async Task<PaginatingResult<OrderViewModel>> GetPaginatedOrdersAsync(int pageNumber, int pageSize)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				if (pageNumber < 1) pageNumber = 1;
				if (pageSize < 1) pageSize = 10;

				var totalCount = _unitOfWork.GetRepository<Order>().Count();

				var orders = await _unitOfWork.GetRepository<Order>()
					.GetPaginateAsync(pageNumber, pageSize, null, o => o.OrderByDescending(o => o.OrderDate), "User");

				var orderViewModels = orders.Select(order =>
				{
					var orderVM = _mapper.Map<OrderViewModel>(order);
					orderVM.User = _mapper.Map<UserViewModel>(order.User);
					return orderVM;
				}).ToList();

				_unitOfWork.CommitTransaction();
				return new PaginatingResult<OrderViewModel>(orderViewModels, pageNumber, totalCount, pageSize);
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

		// Lấy theo status hoặc lấy hết
		public List<OrderViewModel> GetOrdersByUserIdAsync(int userId, OrderStatusEnum? orderStatus)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var orders = _unitOfWork.GetRepository<Order>();
				if (orderStatus.HasValue)
				{
					orders.GetAll(o => o.UserId == userId && o.Status == orderStatus.ToString());
				}
				else
				{
					orders.GetAll(o => o.UserId == userId);
				}

				var orderViewModels = _mapper.Map<List<OrderViewModel>>(orders);

				_unitOfWork.CommitTransaction();
				return orderViewModels;
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				throw;
			}
		}

        public async Task<string> CreateOrderAsync(int userId, OrderDetailViewModel orderDetailViewModel)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var orderRepository = _unitOfWork.GetRepository<Order>();
                var orderDetailRepository = _unitOfWork.GetRepository<OrderDetail>();

                // Kiểm tra xem user đã có đơn hàng "InProgress" hay chưa
                var existingOrder = await orderRepository
                    .GetFirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatusEnum.InProgress.ToString());

                if (existingOrder == null)
                {
                    // Nếu chưa có, tạo đơn hàng mới
                    existingOrder = new Order
                    {
                        UserId = userId,
                        OrderDate = DateTime.UtcNow,
                        Status = OrderStatusEnum.InProgress.ToString(),
                        TotalAmount = 0,
                        FinalAmount = 0
                    };

                    await orderRepository.AddAsync(existingOrder);
                    await _unitOfWork.SaveChangesAsync(); 
                }

				// Thêm sản phẩm vào đơn hàng hiện có hoặc mới tạo
				var orderDetail = _mapper.Map<OrderDetail>(orderDetailViewModel);

                await orderDetailRepository.AddAsync(orderDetail);

                // Cập nhật tổng tiền đơn hàng
                existingOrder.TotalAmount += orderDetail.Quantity * orderDetail.UnitPrice;
                existingOrder.FinalAmount = existingOrder.TotalAmount; 

                orderRepository.Update(existingOrder);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.CommitTransaction();
                return "Thêm sản phẩm vào đơn hàng thành công.";
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return $"Lỗi khi thêm sản phẩm vào đơn hàng: {ex.Message}";
            }
        }
    }
}
