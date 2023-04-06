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
                         SenderEmail = (transaction.SenderType == true) ?
                                         _context.Businesses.FirstOrDefault(b => b.Id == transaction.SenderId).Email :
                                         _context.Users.FirstOrDefault(u => u.Id == transaction.SenderId).Email,
                         RecipientEmail = (transaction.RecpientType == true) ?
                                         _context.Businesses.FirstOrDefault(b => b.Id == transaction.RecipientId).Email :
                                         _context.Users.FirstOrDefault(u => u.Id == transaction.RecipientId).Email,
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
                         SenderEmail = (transaction.SenderType == true) ?
                                         _context.Businesses.FirstOrDefault(b => b.Id == transaction.SenderId).Email :
                                         _context.Users.FirstOrDefault(u => u.Id == transaction.SenderId).Email,
                         RecipientEmail = (transaction.RecpientType == true) ?
                                         _context.Businesses.FirstOrDefault(b => b.Id == transaction.RecipientId).Email :
                                         _context.Users.FirstOrDefault(u => u.Id == transaction.RecipientId).Email,
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
        Transaction tempTransac = transact;
        tempTransac.CreatedAt = DateTime.Now;
        _context.Transactions.Add(tempTransac);
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
}