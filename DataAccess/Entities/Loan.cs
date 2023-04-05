using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Loan
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public int? BusinessId { get; set; }

    public decimal? InterestRate { get; set; }

    public DateTime? DateLoaned { get; set; }

    public DateTime? LoanPaid { get; set; }
}
