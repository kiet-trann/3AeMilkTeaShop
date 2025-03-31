using System;
using System.Collections.Generic;

namespace MilkTeaRepository.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public decimal PriceS { get; set; }

    public decimal PriceM { get; set; }

    public decimal PriceL { get; set; }

    public bool? IsAvailableS { get; set; }

    public bool? IsAvailableM { get; set; }

    public bool? IsAvailableL { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
