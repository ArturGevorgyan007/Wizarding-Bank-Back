using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Business
{
    public int Id { get; set; }

    public string? BusinessName { get; set; }

    public string? Username { get; set; }

    public string Password { get; set; } = null!;

    public string Bin { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Email { get; set; }

    public decimal? Wallet { get; set; }

    public string? BusinessType { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    public virtual ICollection<Card> Cards { get; } = new List<Card>();

    public virtual ICollection<Loan> Loans { get; } = new List<Loan>();
}
