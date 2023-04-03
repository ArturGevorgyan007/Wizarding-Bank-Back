﻿using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public int? CardId { get; set; }

    public int? AccountId { get; set; }

    public int? LoanId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Description { get; set; }

    // public virtual Account? Account { get; set; }

    // public virtual Card? Card { get; set; }

    // public virtual Loan? Loan { get; set; }
}