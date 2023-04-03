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
        return (List<Transaction>)_context.Transactions.Where(w => w.SenderId == id || w.RecipientId == id).ToList();
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