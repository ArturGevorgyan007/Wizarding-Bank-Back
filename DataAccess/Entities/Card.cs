using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Card
{
    public int Id { get; set; }

    public string? CardNumber { get; set; }

    public int? UserId { get; set; }

    public int? BusinessId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Cvv { get; set; }

    public decimal? Balance { get; set; }

    public virtual Business? Business { get; set; }

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
