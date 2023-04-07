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

    
    public Card updateCard(Card card) 
    {
            _context.Cards.Update(card);
           return card; 
    }

    public Card updateCardBalance(int cId, decimal? amt){
        var card = _context.Cards.FirstOrDefault(c => c.Id == cId);
        if (card != null)
        {
            card.Balance += amt; 
            _context.SaveChanges();
            return card;
        }

        return null;
    }

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

    public Card GetCard(int cardId) {
        return _context.Cards.FirstOrDefault(card => card.Id == cardId)!;
    }


}