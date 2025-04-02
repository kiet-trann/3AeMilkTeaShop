using System;
using System.Collections.Generic;

namespace MilkTea.Repository.Model;

public partial class OrderDetailTopping
{
    public int Id { get; set; }

    public int OrderDetailId { get; set; }

    public int ToppingId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual OrderDetail OrderDetail { get; set; } = null!;

    public virtual Topping Topping { get; set; } = null!;
}
