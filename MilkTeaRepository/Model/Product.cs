using System;
using System.Collections.Generic;

namespace MilkTea.Repository.Model;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public decimal PriceS { get; set; }

    public decimal PriceM { get; set; }

    public decimal PriceL { get; set; }

    public bool IsAvailableS { get; set; }

    public bool IsAvailableM { get; set; }

    public bool IsAvailableL { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
