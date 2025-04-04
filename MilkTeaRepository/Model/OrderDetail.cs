﻿using System;
using System.Collections.Generic;

namespace MilkTea.Repository.Model;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string Size { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<OrderDetailTopping> OrderDetailToppings { get; set; } = new List<OrderDetailTopping>();

    public virtual Product Product { get; set; } = null!;
}
