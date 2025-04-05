using MilkTea.Core.Enum;
using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;

namespace MilkTea.Services.OrderServices
{
	public interface IOrderService
	{
        Task<PaginatingResult<OrderViewModel>> GetPaginatedOrdersAsync(int pageNumber, int pageSize);
        Task<OrderViewModel> GetOrderByIdAsync(int orderId);
		List<OrderViewModel> GetOrdersByUserId(int userId, OrderStatusEnum? orderStatus);
		Task<List<OrderDetailViewModel>> GetOrderDetailsForOrder(int orderId);
        Task<string> UpdateOrderStatusAsync(int orderId, string newStatus);
        Task<string> AddProductToOrderAsync(int userId, OrderDetailViewModel orderDetailViewModel,
                                           OrderDetailToppingViewModel? toppingViewModel);

        Task<string> PurchaseOrderAsync(int orderId);

	}
}
