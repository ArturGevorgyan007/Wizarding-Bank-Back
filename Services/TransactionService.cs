using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class TransactionServices
{
    private readonly WizardingBankDbContext _context;
    private readonly UserServices _uservice; 
    private readonly CardServices _cservice; 
    private readonly AccountServices _aservice; 

    public TransactionServices(WizardingBankDbContext context)
    {
        _context = context;
        _uservice = new UserServices(_context);
        _aservice = new AccountServices(_context);
        _cservice = new CardServices(_context);
    }

    public List<Transaction> GetAllTransactions()
    {
        return _context.Transactions.ToList();
    }
    public List<Transaction> GetTransactionsByUserId(int id)
    {
        return (List<Transaction>)_context.Transactions.Where(w => w.SenderId == id || w.RecipientId == id).OrderByDescending(w => w.CreatedAt).ToList();
    }
    public List<Object> GetLimitedTransactionsByUserId(int id)
    {
        List<Object> transactions = new List<Object>();

        var result = from transaction in _context.Transactions
                    join sender in _context.Users on transaction.SenderId equals sender.Id into senderGroup
                    from sender in senderGroup.DefaultIfEmpty()
                    join recipient in _context.Users on transaction.RecipientId equals recipient.Id into recipientGroup
                    from recipient in recipientGroup.DefaultIfEmpty()
                    where transaction.RecipientId == id || transaction.SenderId == id
                    orderby transaction.CreatedAt descending
                    select new
                    {
                        transaction.Id,
                        transaction.Amount,
                        transaction.CreatedAt,
                        SenderEmail = sender.Email,
                        RecipientEmail = recipient.Email,
                        transaction.Description
                    };
        var results = result.Take(10).ToList();
        foreach (Object obj in results)
        {
            transactions.Add(obj);
            Console.WriteLine(obj);
        }
        return transactions;
    }
    public List<Object> GetTransactionsWithEmails(int id)
    {

        // Type transac = {
        //     int id
        // }
        List<Object> transactions = new List<Object>();

        var result = from transaction in _context.Transactions
                    join sender in _context.Users on transaction.SenderId equals sender.Id into senderGroup
                    from sender in senderGroup.DefaultIfEmpty()
                    join recipient in _context.Users on transaction.RecipientId equals recipient.Id into recipientGroup
                    from recipient in recipientGroup.DefaultIfEmpty()
                    where transaction.RecipientId == id || transaction.SenderId == id
                    orderby transaction.CreatedAt
                    select new
                    {
                        transaction.Id,
                        transaction.Amount,
                        transaction.CreatedAt,
                        SenderEmail = sender.Email,
                        RecipientEmail = recipient.Email,
                        transaction.Description
                    };
        foreach (Object obj in result)
        {
            transactions.Add(obj);
            Console.WriteLine(obj);
        }
        return transactions;
    }


    public Transaction CreateTransaction(Transaction transact)
    {
        _context.Transactions.Add(transact);
        _context.SaveChanges();

        return transact;
    }


    public Transaction UpdateTransaction(Transaction transact)
    {
        _context.Transactions.Update(transact);
        _context.Transactions.ToList();
        _context.SaveChanges();

        return transact;
    }

    public Transaction DeleteTransaction(Transaction transact)
    {
        _context.Transactions.Remove(transact);
        _context.SaveChanges();

        return transact;
    }

    public Transaction walletToAccount(Transaction transact){
        UserServices uService = new UserServices(_context);
        AccountServices aService = new AccountServices(_context);
        
        User user = _uservice.GetUser((int)transact.SenderId!);
        Account account = _aservice.getAccountById((int)transact.AccountId!);

        
        if(user.Wallet >= transact.Amount){
            user.Wallet -= transact.Amount;
            account.Balance += transact.Amount;
            _uservice.UpdateWallet(user.Id, user.Wallet);
            _aservice.updateAccountBalance(account.Id,account.Balance);
        }

        _context.Transactions.Add(transact);
        _context.SaveChanges();

        return transact;
    }

    public Transaction walletToCard(Transaction transact){
          
        User user = _uservice.GetUser((int)transact.SenderId!);
        Card card = _cservice.GetCard((int)transact.AccountId!);

        
        if(user.Wallet >= transact.Amount){
            user.Wallet -= transact.Amount;
            card.Balance += transact.Amount;
            _uservice.UpdateWallet(user.Id, user.Wallet);
            _aservice.updateAccountBalance(card.Id,card.Balance);
        }

        _context.Transactions.Add(transact);
        _context.SaveChanges();

        return transact;
    }


    public Transaction acctToWallet(Transaction transact){
        Account acct = _aservice.getAccountById((int)transact.AccountId!);

        if(acct.Balance >= transact.Amount){
            acct.Balance -= transact.Amount;
            User user = _uservice.GetUser((int) transact.SenderId!);
            user.Wallet += transact.Amount; 
            _uservice.UpdateWallet(user.Id, user.Wallet);
            _aservice.updateAccountBalance(acct.Id, acct.Balance);

            _context.Transactions.Add(transact);
            _context.SaveChanges();

            return transact;
        }

        return null;
    }

    public Transaction cardToWallet(Transaction? transact){
        Card? card = _cservice.GetCard((int)transact.CardId!);

        if(card.Balance >= transact.Amount){
            card.Balance -= transact.Amount;
            User user = _uservice.GetUser((int) transact.SenderId!);
            user.Wallet += transact.Amount; 
            _uservice.UpdateWallet(user.Id, user.Wallet);
            _cservice.updateCardBalance(card.Id, card.Balance);

            _context.Transactions.Add(transact);
            _context.SaveChanges();

            return transact;
        }

        return null;
    }
}