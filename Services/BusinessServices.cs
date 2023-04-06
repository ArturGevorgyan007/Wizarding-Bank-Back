using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class BusinessServices
{

    private readonly WizardingBankDbContext _context;

    public BusinessServices(WizardingBankDbContext context)
    {
        _context = context;
    }


    public List<Business> GetAllBusinesses()
    {
        return _context.Businesses.ToList();
    }

    public Business CreateBusiness(Business bus)
    {
        _context.Add(bus);
        _context.SaveChanges();
        return bus;
    }

    public Business UpdateBusiness(Business bus)
    {
        _context.Update(bus);
        _context.Businesses.ToList();
        _context.SaveChanges();
        return bus;
    }

    public Business DeleteBusiness(Business bus)
    {
        _context.Remove(bus);
        _context.SaveChanges();
        return bus;
    }
    public Business GetBusiness(string email)
    {
        List<Business> Found = (List<Business>)_context.Businesses.Where(w => w.Email == email).ToList();
        return Found[0];
    }
}