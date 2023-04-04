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
    public List<Transaction> GetLimitedTransactionsByUserId(int id)
    {
        return (List<Transaction>)_context.Transactions.Where(w => w.SenderId == id || w.RecipientId == id).OrderByDescending(w => w.CreatedAt).Take(10).ToList();
    }
    public List<Object> GetTransactionsWithEmails(int id)
    {
        var userId = 1;

        // Type transac = {
        //     int id
        // }
        List<Object> transactions = new List<Object>();

        var result = from transaction in _context.Transactions
                     join sender in _context.Users on transaction.SenderId equals sender.Id into senderGroup
                     from sender in senderGroup.DefaultIfEmpty()
                     join recipient in _context.Users on transaction.RecipientId equals recipient.Id into recipientGroup
                     from recipient in recipientGroup.DefaultIfEmpty()
                     where transaction.RecipientId == userId || transaction.SenderId == userId
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
}