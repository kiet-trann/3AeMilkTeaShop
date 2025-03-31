using System;
using System.Collections.Generic;

namespace MilkTeaRepository.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string? Size { get; set; }

    public string? SugarLevel { get; set; }

    public string? IceLevel { get; set; }

    public string? SpecialRequest { get; set; }

    public decimal SubTotal { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<OrderDetailTopping> OrderDetailToppings { get; set; } = new List<OrderDetailTopping>();

    public virtual Product? Product { get; set; }
}
