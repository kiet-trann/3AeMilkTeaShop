using System;
using System.Collections.Generic;

namespace MilkTeaRepository.Models;

public partial class Topping
{
    public int ToppingId { get; set; }

    public string ToppingName { get; set; } = null!;

    public decimal Price { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual ICollection<OrderDetailTopping> OrderDetailToppings { get; set; } = new List<OrderDetailTopping>();
}
