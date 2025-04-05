using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTea.Core.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Note { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; } = new();
        public UserViewModel User { get; set; }

		public void CalculateTotalAmount()
		{
			TotalAmount = OrderDetails.Sum(od => od.UnitPrice * od.Quantity);
		}
	}
}
