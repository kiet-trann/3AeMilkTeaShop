
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkTea.Repository.Model;

namespace MilkTeaAdminWeb
{
    public class ordersModel : PageModel
    {
        private readonly ThreeBrothersMilkTeaShopContext _context;
        public ordersModel(ThreeBrothersMilkTeaShopContext context)
        {
            _context = context;
        }

        public void OnGet(int? orderId)
        {
            OrderList = _context.Orders.Include(o => o.User).ToList();

            if (orderId.HasValue)
            {
                SelectedOrder = _context.Orders.Include(o => o.User).FirstOrDefault(o => o.OrderId == orderId);
                OrderDetails = _context.OrderDetails.Include(od => od.Product).Where(od => od.OrderId == orderId).ToList();
            }
        }
        public List<Order> OrderList { get; set; }
        public Order SelectedOrder { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public IActionResult OnPostUpdateStatus(int orderId, string newStatus)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = newStatus;
            _context.SaveChanges();

            return RedirectToPage("./Orders", new { orderId = orderId });
        }
    }
}
