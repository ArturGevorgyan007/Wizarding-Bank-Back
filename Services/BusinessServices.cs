using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class BusinessServices{

    private readonly WizardingBankDbContext _context;

    public BusinessServices(WizardingBankDbContext context){
        _context = context;
    }


    public List<Business> GetAllBusinesses(){
        return _context.Businesses.ToList();
    }

    public Business CreateBusiness(Business bus){
        _context.Add(bus);
        _context.SaveChanges();
        return bus;
    }

    public Business UpdateBusiness(Business bus){
        _context.Update(bus);
        _context.Businesses.ToList();
        _context.SaveChanges();
        return bus;
    }

    public Business DeleteBusiness(Business bus){
        _context.Remove(bus);
        _context.SaveChanges();
        return bus;
    }
}