using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class CardServices
{
    private readonly WizardingBankDbContext _context;

    public CardServices(WizardingBankDbContext context) {
        this._context = context;
    }

    // create a card
    public Card AddCard(Card card) {
        _context.Cards.Add(card);

        _context.SaveChanges();

        return card;
    }

    // delete a card
    public Card RemoveCard(Card card) {
        _context.Cards.Remove(card);

        _context.SaveChanges();

        return card;
    }

    public List<Card> UserCards(int userId) {
        return _context.Cards.Where(card => card.UserId == userId).ToList();
    }

    public List<Card> BusinessCards(int businessId) {
        return _context.Cards.Where(card => card.BusinessId == businessId).ToList();
    }
    
}