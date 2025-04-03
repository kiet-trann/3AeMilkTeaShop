using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkTea.Repository.Model;

namespace MilkTeaAdminWeb.Pages.Orders
{
    public class OrderDetailModel : PageModel
    {
        private readonly ThreeBrothersMilkTeaShopContext _context;
        public OrderDetailModel(ThreeBrothersMilkTeaShopContext context)
        {
            _context = context;
        }
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<OrderDetailTopping> OrderDetailToppings { get; set; }
        public List<Topping> Toppings { get; set; }

        public void OnGet(int orderId)
        {
            Order = _context.Orders.Include(o => o.User).FirstOrDefault(o => o.OrderId == orderId);
            OrderDetails = _context.OrderDetails.Include(od => od.Product).Where(od => od.OrderId == orderId).ToList();

            var orderDetailIds = OrderDetails.Select(od => od.OrderDetailId).ToList();
            OrderDetailToppings = _context.OrderDetailToppings
                    .Include(odt => odt.Topping)
                    .Where(odt => orderDetailIds.Contains(odt.OrderDetailId))
                    .ToList();

            Toppings = _context.Toppings.ToList();
        }
    }
}
