using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string? AccountNumber { get; set; }

    public string? RoutingNumber { get; set; }

    public int? UserId { get; set; }

    public int? BusinessId { get; set; }

    public decimal? Balance { get; set; }

    public virtual Business? Business { get; set; }

    public virtual User? User { get; set; }
}
