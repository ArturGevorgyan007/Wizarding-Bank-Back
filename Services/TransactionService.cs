using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class TransactionServices
{
    private readonly WizardingBankDbContext _context;


    public TransactionServices(WizardingBankDbContext context)
    {
        _context = context;
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
        
        User user =this.getUser((int)transact.SenderId!);
        Account account = this.getAccountById((int)transact.AccountId!);

        
        if(user.Wallet >= transact.Amount){
            this.updateWallet(user.Id, -transact.Amount);
            this.updateAccountBalance(account.Id, transact.Amount);
        }

        _context.Transactions.Add(transact);
        _context.SaveChanges();

        return transact;
    }

    public Transaction walletToCard(Transaction transact){
          
        User user = this.getUser((int)transact.SenderId!);
        
        if(user.Wallet >= transact.Amount){
            
            this.updateWallet(user.Id, -transact.Amount);
            
            Card card = this.GetCard((int)transact.CardId!);
    
            this.updateCardBalance(card.Id, transact.Amount);
            _context.Transactions.Add(transact);
        }

       
        _context.SaveChanges();

        return transact;
    }


    public  Transaction acctToWallet(Transaction transact){
        Account acct = this.getAccountById((int)transact.AccountId!);

        if(acct.Balance >= transact.Amount){
            this.updateAccountBalance(acct.Id, -transact.Amount);
            User user = this.getUser((int) transact.RecipientId!);
            this.updateWallet(user.Id, transact.Amount);
            

            _context.Transactions.Add(transact);
            _context.SaveChanges();

            return transact;
        }

        return null;
    }

    public Transaction cardToWallet(Transaction? transact){
        Card? card = this.GetCard((int)transact.CardId!);

        if(card.Balance >= transact.Amount){
            User user = this.getUser((int) transact.RecipientId!);
            this.updateWallet(user.Id, transact.Amount);
            this.updateCardBalance(card.Id, -transact.Amount);

            _context.Transactions.Add(transact);
            _context.SaveChanges();

            return transact;
        }

        return null;
    }

    public User updateWallet(int id, decimal? ammount)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            user.Wallet += ammount;
            return user;
        }
        return null;
    }

    public User getUser(int id){
        return _context.Users.FirstOrDefault(w => w.Id == id)!;
    }

     public Card updateCardBalance(int cId, decimal? amt){
        var card = _context.Cards.FirstOrDefault(c => c.Id == cId);
        if (card != null)
        {
            card.Balance += amt; 
            return card;
        }

        return null;
    }

    public Card GetCard(int cardId) {
        return _context.Cards.FirstOrDefault(card => card.Id == cardId)!;
    }


    public Account updateAccountBalance(int id, decimal? bal)
    {
        var account = _context.Accounts.FirstOrDefault(a => a.Id == id);
        if (account != null)
        {
            account.Balance += bal;
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
}