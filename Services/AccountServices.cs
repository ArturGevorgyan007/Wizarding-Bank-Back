using DataAccess.Entities;

namespace Services;

public class AccountServices{

    private readonly WizardingBankDbContext _context;

    public AccountServices(WizardingBankDbContext context){
        _context = context;
    }

    public Account createAccount(Account acct){
        _context.Add(acct);
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        return acct;
    }

    public List<Account> getAllAccounts(){
        return _context.Accounts.ToList();
    }

    public List<Account> getAccounts(int Id){
        List<Account> acctL = new();
        foreach(Account a in _context.Accounts.ToList()){
            if(a.BusinessId == Id || a.UserId == Id){
                acctL.Add(a);
            }
        }

        return acctL;
    }

    public Account updateAccount(Account acct){
        _context.Update(acct);
        _context.Accounts.ToList();
        _context.SaveChanges();
        return acct;
    }

    public Account updateAccountBalance(int id, decimal? bal)
    {
        var account = _context.Accounts.FirstOrDefault(a => a.Id == id);
        if (account != null)
        {
            account.Balance += bal;
            _context.SaveChanges();
            return account;
        }
        return null;
    }


    //get Account by accountid
    public Account getAccountById(int id){
        var account = _context.Accounts.FirstOrDefault(a => a.Id == id);

        if(account != null){
            return account;
        }
        return null;
    }

    public bool deleteAccount(int acctId, int Id){
        List<Account> acctl = getAccounts(Id);
        Account dacct = new();
        foreach(Account a in acctl){
            if(a.Id == acctId){ 
                dacct = a;}
        }
        _context.Remove(dacct);
        if(_context.SaveChanges() > 0){
            return true;
        }

        return false;
        
    }


}