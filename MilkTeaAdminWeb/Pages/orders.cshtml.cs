using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTea.Services.OrderServices; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkTeaAdminWeb
{
    public class ordersModel : PageModel
    {
        private readonly IOrderService _orderService; 

        public ordersModel(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        public async Task OnGet(int? orderId)
        {
            OrderList = (List<OrderViewModel>)await _orderService.GetPaginatedOrdersAsync(1, int.MaxValue); 

            if (orderId.HasValue)
            {
                SelectedOrder = await _orderService.GetOrderByIdAsync(orderId.Value);
                OrderDetails = await _orderService.GetOrderDetailsForOrder(orderId.Value);
            }
        }

        public List<OrderViewModel> OrderList { get; set; } 
        public OrderViewModel SelectedOrder { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; } = new();

        public async Task<IActionResult> OnPostUpdateStatus(int orderId, string newStatus)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, newStatus); 
            return RedirectToPage("./Orders", new { orderId = orderId });
        }
    }
}