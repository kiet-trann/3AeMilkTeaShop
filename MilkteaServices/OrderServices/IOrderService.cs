using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;

namespace MilkTea.Services.OrderServices
{
	public interface IOrderService
	{
        Task<IEnumerable<OrderViewModel>> GetPaginatedOrdersAsync(int pageIndex, int pageSize);
        Task<OrderViewModel> GetOrderByIdAsync(int orderId);
        Task<List<OrderDetailViewModel>> GetOrderDetailsForOrder(int orderId);
        Task<string> UpdateOrderStatusAsync(int orderId, string newStatus);

    }
}
