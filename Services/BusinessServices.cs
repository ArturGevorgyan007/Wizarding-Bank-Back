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

    public List<Business> getBusinessById(int businessId){
        return _context.Businesses.Where(w => w.Id == businessId).ToList();
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
    public Business? GetBusiness(string email)
    {
        var busi = _context.Businesses.FirstOrDefault(u => u.Email== email);
        if( busi != null){
            return busi;
        }
        return null;
    }

    public Business? updateBWallet(int id, decimal? amt){
        var user = _context.Businesses.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            user.Wallet += amt;
            _context.SaveChanges();
            return user;
        }
        return null;
    }
}