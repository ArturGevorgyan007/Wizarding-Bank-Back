using DataAccess.Entities;

namespace Services;
public class LoanServices
{
    private readonly WizardingBankDbContext _context;

    public LoanServices(WizardingBankDbContext context)
    {
        _context = context;
    }
    public Loan CreateBusinessLoan(Loan loan)
    {
        Loan loanPH = loan;
        loanPH.LoanPaid = null;
        loanPH.LoanPaid = DateTime.Now;
        _context.Loans.Add(loanPH);
        var bus = _context.Businesses.SingleOrDefault(x => x.Id == loan.BusinessId);
        bus!.Wallet += loan.Amount;
        _context.SaveChanges();

        // _context.ChangeTracker.Clear();
        return loan;
    }
    public List<Loan> GetAllBusinessLoan(int business_id)
    {
        return _context.Loans.Where(x => x.BusinessId == business_id && (x.Amount - x.AmountPaid) > 0).ToList();
    }
    public Loan PayLoan(int id, decimal principle, decimal amount)
    {
        Console.WriteLine("Id: " + id + " Amount: " + amount);
        var loanObj = _context.Loans.SingleOrDefault(x => x.BusinessId == id && x.Amount - x.AmountPaid > 0);
        var bus = _context.Businesses.SingleOrDefault(y => y.Id == id);
        if (loanObj!.Amount - principle <= 0)
        {
            bus!.Wallet -= loanObj.Amount - loanObj.AmountPaid;
            loanObj.AmountPaid = loanObj.Amount;
            loanObj.LoanPaid = DateTime.Now;
        }
        else
        {
            loanObj.AmountPaid += principle;
            bus!.Wallet -= amount;
            if (principle != amount)
            {
                DateTime payment = (DateTime)loanObj.LoanPaid;
                payment = payment.AddMonths(1);
                loanObj.LoanPaid = payment;

            }
        }
        _context.SaveChanges();
        return loanObj;

    }
}