using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Services.OrderServices;

namespace MilkTeaAdminWeb.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public PaginatingResult<OrderViewModel> PaginatedOrders { get; set; } = default!;

        public int PageSize { get; set; } = 5;

        public OrderViewModel SelectedOrder { get; set; }

        public List<OrderDetailViewModel> OrderDetails { get; set; } = new();

        public async Task OnGetAsync(int pageIndex = 1, int? orderId = null)
        {
            // Lấy các đơn hàng theo phân trang
            PaginatedOrders = await _orderService.GetPaginatedOrdersAsync(pageIndex, PageSize);

            // Nếu có orderId, lấy chi tiết đơn hàng tương ứng
            if (orderId.HasValue)
            {
                SelectedOrder = await _orderService.GetOrderByIdAsync(orderId.Value);
                OrderDetails = await _orderService.GetOrderDetailsForOrder(orderId.Value);
            }
        }

        public async Task<IActionResult> OnPostUpdateStatus(int orderId, string newStatus, int pageIndex = 1)
        {
            // Cập nhật trạng thái đơn hàng
            await _orderService.UpdateOrderStatusAsync(orderId, newStatus);

            // Quay lại trang đơn hàng với trạng thái cập nhật
            return RedirectToPage("./Index", new { pageIndex });
        }
    }
}
