using System;
using System.Collections.Generic;

namespace MilkTeaRepository.Models;

public partial class OrderDetailTopping
{
    public int Id { get; set; }

    public int? OrderDetailId { get; set; }

    public int? ToppingId { get; set; }

    public int? Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual OrderDetail? OrderDetail { get; set; }

    public virtual Topping? Topping { get; set; }
}
