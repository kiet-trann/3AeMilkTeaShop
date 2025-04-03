namespace MilkTea.Core.ViewModels
{
    public class OrderDetailToppingViewModel
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
