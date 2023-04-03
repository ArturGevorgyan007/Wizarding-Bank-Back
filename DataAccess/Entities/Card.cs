using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Card
{
    public int Id { get; set; }

    public long CardNumber { get; set; }

    public int? UserId { get; set; }

    public int? BusinessId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int Cvv { get; set; }

    public decimal? Balance { get; set; }

    public virtual Business? Business { get; set; }

    public virtual User? User { get; set; }
}
