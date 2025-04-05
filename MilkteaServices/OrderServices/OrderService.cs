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
        public List<OrderViewModel> GetOrdersByUserId(int userId, OrderStatusEnum? orderStatus = null)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var orderRepository = _unitOfWork.GetRepository<Order>();

                // Lọc đơn hàng theo userId và trạng thái nếu có
                var orders = orderStatus.HasValue
                    ? orderRepository.GetAll(o => o.UserId == userId && o.Status == orderStatus.ToString())
                    : orderRepository.GetAll(o => o.UserId == userId);

                // Biến để chứa list OrderViewModel
                var orderViewModels = new List<OrderViewModel>();

                // Duyệt qua từng đơn hàng và chuyển sang OrderViewModel
                foreach (var order in orders)
                {
                    var orderViewModel = _mapper.Map<OrderViewModel>(order);

                    var orderDetails = _unitOfWork.GetRepository<OrderDetail>()
                        .GetAll(od => od.OrderId == order.OrderId);

                    orderViewModel.OrderDetails = _mapper.Map<List<OrderDetailViewModel>>(orderDetails);

                    orderViewModel.User = _mapper.Map<UserViewModel>(order.User);

                    orderViewModels.Add(orderViewModel);
                }

                _unitOfWork.CommitTransaction();

                return orderViewModels;
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<string> AddProductToOrderAsync(int userId, OrderDetailViewModel orderDetailViewModel,
                                           OrderDetailToppingViewModel? toppingViewModel = null)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var orderRepository = _unitOfWork.GetRepository<Order>();
                var orderDetailRepository = _unitOfWork.GetRepository<OrderDetail>();
                var orderDetailToppingRepository = _unitOfWork.GetRepository<OrderDetailTopping>();

                // Kiểm tra xem user đã có đơn hàng "InProgress" hay chưa
                var existingOrder = await orderRepository
                    .GetFirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatusEnum.InProgress.ToString());

                // Chưa có => tạo Order mới
                if (existingOrder == null)
                {
                    existingOrder = new Order
                    {
                        UserId = userId,
                        OrderDate = DateTime.UtcNow,
                        Status = OrderStatusEnum.InProgress.ToString(),
                        TotalAmount = 0,
                    };

                    await orderRepository.AddAsync(existingOrder);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Thêm sản phẩm vào đơn hàng
                var orderDetail = _mapper.Map<OrderDetail>(orderDetailViewModel);
                orderDetail.OrderId = existingOrder.OrderId;

                await orderDetailRepository.AddAsync(orderDetail);
                await _unitOfWork.SaveChangesAsync();

                decimal toppingPrice = 0;

                // Nếu có topping => thêm OrderDetailTopping
                if (toppingViewModel != null)
                {
                    var orderDetailTopping = _mapper.Map<OrderDetailTopping>(toppingViewModel);
                    orderDetailTopping.OrderDetailId = orderDetail.OrderDetailId;

                    await orderDetailToppingRepository.AddAsync(orderDetailTopping);
                    toppingPrice = orderDetailTopping.Price;
                }

                // Cập nhật tổng tiền
                existingOrder.TotalAmount += (orderDetail.Quantity * orderDetail.UnitPrice) + toppingPrice;

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

        public async Task<string> PurchaseOrderAsync(int orderId)
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

                if (order.Status != OrderStatusEnum.InProgress.ToString())
                {
                    _unitOfWork.CommitTransaction();
                    return "Đơn hàng không thể hoàn thành vì không phải trạng thái 'InProgress'.";
                }

                order.Status = OrderStatusEnum.Completed.ToString();

                _unitOfWork.GetRepository<Order>().Update(order);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.CommitTransaction();

                return "Đơn hàng đã được hoàn thành và thanh toán thành công.";
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return $"Lỗi khi hoàn thành đơn hàng: {ex.Message}";
            }
        }
    }
}
