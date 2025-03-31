﻿using System;
using System.Collections.Generic;

namespace MilkTeaRepository.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int? Points { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
