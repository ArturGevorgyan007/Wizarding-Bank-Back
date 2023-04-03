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
        _context.Add(loan);
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        return loan;
    }
    public List<Loan> GetAllBusinessLoan(Loan loan) 
    {
        return _context.Loans.Where(x => x.BusinessId == loan.BusinessId).ToList();
    }
    public Loan PayLoan(Loan loan, int amount){
        var loanObj = _context.Loans.SingleOrDefault(x => x.BusinessId == loan.BusinessId);
        if(loanObj.Amount - amount <= 0){
            loanObj.Amount = 0;
            loanObj.LoanPaid = DateTime.Now;
        }
        else{
            loanObj.Amount = loanObj.Amount - amount;
        }
        _context.SaveChanges();
        return loanObj;
        
    }
}