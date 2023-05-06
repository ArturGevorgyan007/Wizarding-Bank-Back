using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Username { get; set; }

    public string Password { get; set; } = null!;

    public string? Address { get; set; }

    public string Email { get; set; } = null!;

    public decimal? Wallet { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    public virtual ICollection<Card> Cards { get; } = new List<Card>();
}
