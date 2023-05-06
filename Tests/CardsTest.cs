using DataAccess.Entities;
using Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class CardsTest
    {
        Card card = new Card
        {
            Id = 1,
            CardNumber = 1111222233334444,
            ExpiryDate = DateTime.Now,
            BusinessId = 1,
            Cvv = 123,
            Balance = 0.00m
        };
        Card card2 = new Card
        {
            Id = 2,
            CardNumber = 1191982239734684,
            ExpiryDate = DateTime.Now,
            UserId = 1,
            Cvv = 777,
            Balance = 1.00m
        };

        [Fact]
        public void shouldCreateCardForBusinessIDOfOne()
        {

            var cardList = new List<Card>{
                card
            };

            var cardQueryable = cardList.AsQueryable();

            var cardsDbSetMock = new Mock<DbSet<Card>>();
            cardsDbSetMock.As<IQueryable<Card>>().Setup(x => x.Provider).Returns(cardQueryable.Provider);
            cardsDbSetMock.As<IQueryable<Card>>().Setup(x => x.Expression).Returns(cardQueryable.Expression);
            cardsDbSetMock.As<IQueryable<Card>>().Setup(x => x.ElementType).Returns(cardQueryable.ElementType);
            cardsDbSetMock.As<IQueryable<Card>>().Setup(x => x.GetEnumerator()).Returns(cardQueryable.GetEnumerator);

            var contextMock = new Mock<WizardingBankDbContext>();
            contextMock.Setup(x => x.Cards).Returns(cardsDbSetMock.Object);

            var cardServices = new CardServices(contextMock.Object);

            var result = cardServices.AddCard(card);

            Assert.Equal(1, result.Id);
            Assert.Equal(1111222233334444, result.CardNumber);
            Assert.Equal(123, result.Cvv);
            Assert.Equal(0.00m, result.Balance);
            Assert.Equal(1, result.BusinessId);
        }

        [Fact]
        public void shouldCreateCardForUserIDOfOne()
        {

            var cardList = new List<Card>{
                card2
            };

            var cardQueryable = cardList.AsQueryable();

            var cardsDbSetMock = new Mock<DbSet<Card>>();
            cardsDbSetMock.As<IQueryable<Card>>().Setup(x => x.Provider).Returns(cardQueryable.Provider);
            cardsDbSetMock.As<IQueryable<Card>>().Setup(x => x.Expression).Returns(cardQueryable.Expression);
            cardsDbSetMock.As<IQueryable<Card>>().Setup(x => x.ElementType).Returns(cardQueryable.ElementType);
            cardsDbSetMock.As<IQueryable<Card>>().Setup(x => x.GetEnumerator()).Returns(cardQueryable.GetEnumerator);

            var contextMock = new Mock<WizardingBankDbContext>();
            contextMock.Setup(x => x.Cards).Returns(cardsDbSetMock.Object);

            var cardServices = new CardServices(contextMock.Object);

            var result = cardServices.AddCard(card2);

            Assert.Equal(2, result.Id);
            Assert.Equal(1191982239734684, result.CardNumber);
            Assert.Equal(777, result.Cvv);
            Assert.Equal(1.00m, result.Balance);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public void UserCards_ShouldReturnListOfUserCards()
        {
            // Arrange
            var mockContext = new Mock<WizardingBankDbContext>();
            var cardServices = new CardServices(mockContext.Object);
            var userId = 1;
            var cards = new List<Card>
            {
                new Card { Id = 1, UserId = 1 },
                new Card { Id = 2, UserId = 1 },
                new Card { Id = 3, UserId = 2 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Card>>();
            mockSet.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(cards.Provider);
            mockSet.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(cards.Expression);
            mockSet.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(cards.ElementType);
            mockSet.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(cards.GetEnumerator());

            mockContext.Setup(c => c.Cards).Returns(mockSet.Object);
            // Act
            var result = cardServices.UserCards(userId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.True(result.All(c => c.UserId == userId));
        }

        [Fact]
        public void UpdateCard_ShouldUpdateCardInContextAndReturnIt()
        {
            // Arrange
            var cards = new List<Card>
            {
                new Card { Id = 1, UserId = 1 },
                new Card { Id = 2, UserId = 1 },
                new Card { Id = 3, UserId = 2 }
            }.AsQueryable();
            var contextMock = new Mock<WizardingBankDbContext>();
            var cardSetMock = new Mock<DbSet<Card>>();
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(cards.Provider);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(cards.Expression);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(cards.ElementType);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(cards.GetEnumerator());

            contextMock.Setup(c => c.Cards).Returns(cardSetMock.Object);
            var cardServices = new CardServices(contextMock.Object);

            // Act
            var result = cardServices.updateCard(card);

            // Assert
            cardSetMock.Verify(cs => cs.Update(card), Times.Once);
            Assert.Equal(card, result);
        }
        [Fact]
        public void BusinessCards_ShouldReturnListOfCards_WhenBusinessIdIsValid()
        {
            // Arrange
            var businessId = 1;
            var cardsList = new List<Card> {
                new Card { Id = 1, BusinessId = businessId },
                new Card { Id = 2, BusinessId = businessId }
            };
            var cards = cardsList.AsQueryable();
            var contextMock = new Mock<WizardingBankDbContext>();
            var cardSetMock = new Mock<DbSet<Card>>();
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(cards.Provider);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(cards.Expression);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(cards.ElementType);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(cards.GetEnumerator());
            contextMock.Setup(db => db.Cards).Returns(cardSetMock.Object);

            // Act
            var cardServices = new CardServices(contextMock.Object);
            var result = cardServices.BusinessCards(businessId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cardsList.Count, result.Count);
            Assert.Equal(cardsList, result);
        }

        [Fact]
        public void GetCard_ShouldReturnCard_WhenCardIdIsValid()
        {
            // Arrange
            var businessId = 1;
            var cardsList = new List<Card> {
                new Card { Id = 1, BusinessId = businessId },
                new Card { Id = 2, BusinessId = businessId }
            };
            var cards = cardsList.AsQueryable();
            var contextMock = new Mock<WizardingBankDbContext>();
            var cardSetMock = new Mock<DbSet<Card>>();
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(cards.Provider);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(cards.Expression);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(cards.ElementType);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(cards.GetEnumerator());
            contextMock.Setup(db => db.Cards).Returns(cardSetMock.Object);
            var cardServices = new CardServices(contextMock.Object);


            // Act
            var result = cardServices.GetCard(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cardsList[0], result);
        }

        [Fact]
        public void UpdateCardBalance_ShouldUpdateCardBalance_WhenCardExists()
        {
            // Arrange
            var cardId = 1;
            var amount = 10m;


            var cardsList = new List<Card> {
                new Card { Id = 1, BusinessId = 1 },
                new Card { Id = 2, BusinessId = 2}
            };

            var cards = cardsList.AsQueryable();
            var contextMock = new Mock<WizardingBankDbContext>();
            var cardSetMock = new Mock<DbSet<Card>>();
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(cards.Provider);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(cards.Expression);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(cards.ElementType);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(cards.GetEnumerator());
            contextMock.Setup(db => db.Cards).Returns(cardSetMock.Object);

            // contextMock.Setup(c => c.Cards).Returns(new[] { card }.AsQueryable());
            var service = new CardServices(contextMock.Object);

            // Act
            var result = service.updateCardBalance(cardId, amount);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cardId, result.Id);
            // Assert.Equal(card.Balance + amount, result.Balance);
            contextMock.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateCardBalance_ShouldReturnNull_WhenCardDoesNotExist()
        {
            // Arrange
            var cardsList = new List<Card> {
                new Card { Id = 1, BusinessId = 1 },
                new Card { Id = 2, BusinessId = 2}
            };

            var cards = cardsList.AsQueryable();
            var contextMock = new Mock<WizardingBankDbContext>();
            var cardSetMock = new Mock<DbSet<Card>>();
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Provider).Returns(cards.Provider);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.Expression).Returns(cards.Expression);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.ElementType).Returns(cards.ElementType);
            cardSetMock.As<IQueryable<Card>>().Setup(m => m.GetEnumerator()).Returns(cards.GetEnumerator());
            contextMock.Setup(db => db.Cards).Returns(cardSetMock.Object);

            var service = new CardServices(contextMock.Object);

            // Act
            var result = service.updateCardBalance(5, 10m);

            // Assert
            Assert.Null(result);
            contextMock.Verify(c => c.SaveChanges(), Times.Never);
        }





    }
}