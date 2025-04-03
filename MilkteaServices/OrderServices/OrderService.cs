using AutoMapper;
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
	}
}
