using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking; 
using Moq;
using Xunit;
using Services;
using System.Linq;

namespace Tests

{
    public class TransactionServicesTests
    {
        private Mock<WizardingBankDbContext> _contextMock;
        private TransactionServices _transactionServices;

        // [SetUp]
        // public void SetUp()
        // {
        //     _contextMock = new Mock<WizardingBankDbContext>();
        //     _transactionServices = new TransactionServices(_contextMock.Object);
        // }

        [Fact]
        public void GetAllTransactions_ShouldReturnAllTransactions()
        {
            // Arrange
        var _contextMock = new Mock<WizardingBankDbContext>();
        var _transactionServices = new TransactionServices(_contextMock.Object);

            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1 },
                new Transaction { Id = 2 },
                new Transaction { Id = 3 }
            };
            var dbSetMock = new Mock<DbSet<Transaction>>();
            dbSetMock.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(transactions.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(transactions.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(transactions.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(transactions.GetEnumerator());
            _contextMock.Setup(c => c.Transactions).Returns(dbSetMock.Object);

            // Act
            var result = _transactionServices.GetAllTransactions();

            // Assert
            Assert.Equal(transactions.Count, result.Count);
            Assert.Equal(transactions[0].Id, result[0].Id);
        }


        [Fact]
        public void GetTransactionByUserID_ReturnsTransactions_WhenTransactionsExist()
        {
            // Arrange
            var userId = 1;
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, SenderId = 1, Amount = 10, Description = "Transaction 1" },
                new Transaction { Id = 2, SenderId = 1, Amount = 20, Description = "Transaction 2" }
            }.AsQueryable();

            // Create a mock DbContext
            var mockContext = new Mock<WizardingBankDbContext>();
            var mockSet = new Mock<DbSet<Transaction>>();
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(transactions.Provider);
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(transactions.Expression);
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(transactions.ElementType);
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(transactions.GetEnumerator());
            mockContext.Setup(c => c.Transactions).Returns(mockSet.Object);

            var service = new TransactionServices(mockContext.Object);

            // Act
            var result = service.GetTransactionsByUserId(userId);

            // Assert
            Assert.Equal(transactions.Count(), result.Count());
            Assert.Equal(transactions.FirstOrDefault().Id, result.FirstOrDefault().Id);
            Assert.Equal(transactions.FirstOrDefault().Amount, result.FirstOrDefault().Amount);
            Assert.Equal(transactions.FirstOrDefault().Description, result.FirstOrDefault().Description);
        }
    
        [Fact]
        public void GetTransactionByUserID_WhenTransactionDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            var userId = 4;
            var transactions = new List<Transaction>();

            var mockSet = new Mock<DbSet<Transaction>>();
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(transactions.AsQueryable().Provider);
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(transactions.AsQueryable().Expression);
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(transactions.AsQueryable().ElementType);
            mockSet.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(transactions.AsQueryable().GetEnumerator());

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(c => c.Transactions).Returns(mockSet.Object);

            var service = new TransactionServices(mockContext.Object);

            // Act
            var result =  service.GetTransactionsByUserId(userId);

            // Assert
            Assert.Empty(result);
        }

        // [Fact]
        // public void CreateTransaction_ShouldAddTransactionToContext()
        // {
        //     // Arrange
        //     var transact = new Transaction { Id = 90, Amount = 10.0m, CreatedAt = DateTime.Now };
        //     var options = new DbContextOptionsBuilder<WizardingBankDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;
        //     var dbContextMock = new Mock<WizardingBankDbContext>(options);
        //     var transactionServices = new TransactionServices(dbContextMock.Object);

        //     // Act
        //     var result = transactionServices.CreateTransaction(transact);

        //     // Assert
        //     dbContextMock.Verify(x => x.Add(transact), Times.Once);
        //     dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        //     Assert.Equal(transact, result);
        // }

        [Fact]
        public void CreateTransaction_WhenCalled_AddsTransactionToContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateTransaction_AddsTransactionToContext")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {
                var transaction = new Transaction()
                {
                    Id = 1,
                    Amount = 100,
                    SenderId = 1,
                    RecipientId = 2,
                    SenderType = true,
                    RecpientType = false,
                    AccountId = null,
                    CardId = null,
                    CreatedAt = DateTime.UtcNow
                };

                var transactionServices = new TransactionServices(context);

                // Act
                var result = transactionServices.CreateTransaction(transaction);

                // Assert
                Assert.Equal(1, context.Transactions.Count());
                Assert.Equal(transaction, context.Transactions.Single());
            }
        }

        [Fact]
        public void UpdateTransaction_Should_Update_Transaction()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new Mock<WizardingBankDbContext>(options);

            var transaction = new Transaction
            {
                Id = 1,
                Amount = 100,
                SenderId = 1,
                RecipientId = 2,
                CreatedAt = DateTime.Now,
                Description = "Test Transaction"
            };

            dbContext.Setup(x => x.Transactions.Update(It.IsAny<Transaction>()));
            dbContext.Setup(x => x.Transactions.Find(transaction.Id)).Returns(transaction);

            var transactionService = new TransactionServices(dbContext.Object);

            // Act
            var result = transactionService.UpdateTransaction(transaction);

            // Assert
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.Equal(transaction, result);
        }

        [Fact]
        public void DeleteTransaction_DeletesTransactionFromDatabase()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1 },
                new Transaction { Id = 2 },
                new Transaction { Id = 3 }
            };
            
            var mockDbContext = new Mock<WizardingBankDbContext>();
            mockDbContext.Setup(x => x.Transactions).Returns(MockDbSet2(transactions));
            
            var transactionService = new TransactionServices(mockDbContext.Object);

            var transactionToDelete = new Transaction { Id = 2 };

            // Act
            transactionService.DeleteTransaction(transactionToDelete);

            // Assert
            Assert.DoesNotContain(transactionToDelete, transactions);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Once);
        }


        // Utility method for mocking DbSet
        private static DbSet<T> MockDbSet2<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockDbSet.Setup(x => x.Add(It.IsAny<T>())).Callback((T entity) => data.Add(entity));
            mockDbSet.Setup(x => x.Remove(It.IsAny<T>())).Callback((T entity) => data.Remove(entity));
            return mockDbSet.Object;
        }


[Fact]
        public void TestCardToWallet()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                // new Transaction { Id = 300, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1, RecipientId = 1, Description = "Transaction 1", CardId = 1 },
                new Transaction { Id = 2, Amount = 75, CreatedAt = new DateTime(2022, 3, 2), SenderId = 2, RecipientId = 2, Description = "Transaction 2", CardId = 2 }
            };

            var users = new List<User>
            {
                new User { Id = 1, Email = "user1@test.com", Password = "Password1", Wallet = 100},
                new User { Id = 2, Email = "user2@test.com", Password = "Password2", Wallet = 200},
            };

            var cards = new List <Card>
            {
                new Card {Id = 1, UserId = 1, CardNumber = 12345612345, Balance = 100, Cvv = 123},
                new Card {Id = 2, UserId = 2, CardNumber = 23456712334, Balance = 200, Cvv = 456}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "CardToWallet_Database")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {
                context.Transactions.AddRange(transactions);
                context.Users.AddRange(users);
                context.Cards.AddRange(cards);
                context.SaveChanges();
            }

            using (var context = new WizardingBankDbContext(options))
            {
                var service = new TransactionServices(context);

                // Act
                var result = service.cardToWallet(new Transaction { Id = 300, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1, RecipientId = 1, Description = "Transaction 1", CardId = 1 });

                // Assert
                Assert.NotNull(result);
                Assert.IsType<Transaction>(result);
                // Assert.Equal(120, users[0].Wallet);
            }

        }

    }
    
}

