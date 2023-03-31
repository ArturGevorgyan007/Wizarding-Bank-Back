using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using DataAccess.Entities;
using Services;

namespace API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase {
        private readonly CardServices _repo;

        public CardController(CardServices repo) {
            this._repo = repo;
        }

        // create a card
        [HttpPost]
        public Card AddCard(Card card) {
            return _repo.AddCard(card);
        }

        // delete a card
        [HttpDelete]
        public Card RemoveCard(Card card) {
            return _repo.RemoveCard(card);
        }

        [HttpGet]
        public List<Card> GetCards(int userId, bool isBusiness) {
            if(isBusiness) return _repo.BusinessCards(userId);
            else return _repo.UserCards(userId);
        }

    }

}