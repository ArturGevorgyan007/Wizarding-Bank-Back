using DataAccess.Entities;

namespace DataAccess;
public class UserServices
{
    private readonly WizardingBankDbContext _context;

    public UserServices(WizardingBankDbContext context){

        _context = context;

    }
        public User CreateUser(User a) {
        _context.Add(a);

        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        return a;
    }

    public List<User> GetAll() {
    return _context.Users.ToList();
    }



}
