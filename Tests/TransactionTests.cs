
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
            var result = service.GetTransactionsByUserId(userId);

            // Assert
            Assert.Empty(result);
        }


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

            var cards = new List<Card>
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


        [Fact]
        public void GetTransactionsByUserId_ShouldReturnTransactionsByUserId()
        {
            // Arrange
            var userId = 1;

            var transactions = new List<Transaction>()
            {
                new Transaction() { Id = 1, SenderId = userId, RecipientId = 2, Amount = 100, CreatedAt = DateTime.Now },
                new Transaction() { Id = 2, SenderId = 3, RecipientId = userId, Amount = 200, CreatedAt = DateTime.Now.AddDays(-1) },
                new Transaction() { Id = 3, SenderId = userId, RecipientId = 4, Amount = 300, CreatedAt = DateTime.Now.AddDays(-2) }
            };
            var _contextMock = new Mock<WizardingBankDbContext>();

            var transactionQueryable = transactions.AsQueryable();
            var transactionsDbSetMock = new Mock<DbSet<Transaction>>();
            transactionsDbSetMock.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(transactionQueryable.Provider);
            transactionsDbSetMock.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(transactionQueryable.Expression);
            transactionsDbSetMock.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(transactionQueryable.ElementType);
            transactionsDbSetMock.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(transactionQueryable.GetEnumerator);

            _contextMock.Setup(c => c.Transactions).Returns(transactionsDbSetMock.Object);

            var transactionServices = new TransactionServices(_contextMock.Object);

            // Act
            var result = transactionServices.GetTransactionsByUserId(userId);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.True(result.All(t => t.SenderId == userId || t.RecipientId == userId));
        }
        [Fact]
        public void GetTransactionsWithEmails_Should_ReturnListOfObjects()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Amount = 50, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1, RecipientId = 2, Description = "Transaction 1" },
                new Transaction { Id = 2, Amount = 75, CreatedAt = new DateTime(2022, 3, 2), SenderId = 2, RecipientId = 1, Description = "Transaction 2" }
            };

            var users = new List<User>
            {
                new User { Id = 1, Email = "user1@test.com", Password = "Password1", },
                new User { Id = 2, Email = "user2@test.com", Password = "Password2" },
            };

            var businesses = new List<Business>
            {
                new Business { Id = 1, Email = "business1@test.com", Address="2613 35th Ave W", Bin = "473234", Password="password"},
                new Business { Id = 2, Email = "business2@test.com", Address="123 Number Rd", Bin =  "1523546", Password="Password1"}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "GetTransactionsWithEmails_Database")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {

                context.Transactions.AddRange(transactions);
                context.Users.AddRange(users);
                context.Businesses.AddRange(businesses);
                context.SaveChanges();
            }

            using (var context = new WizardingBankDbContext(options))
            {
                var service = new TransactionServices(context);

                // Act
                var result = service.GetTransactionsWithEmails(1);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<object>>(result);
                Assert.Equal(2, result.Count);
            }

        }


        [Fact]
        public void GetLimitedTransactionsByUserId_ReturnsLimitedTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Amount = 50, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1, RecipientId = 2, Description = "Transaction 1" },
                new Transaction { Id = 2, Amount = 75, CreatedAt = new DateTime(2022, 3, 2), SenderId = 2, RecipientId = 1, Description = "Transaction 2" }
            };

            var users = new List<User>
            {
                new User { Id = 1, Email = "user1@test.com", Password = "Password1", },
                new User { Id = 2, Email = "user2@test.com", Password = "Password2" },
            };

            var businesses = new List<Business>
            {
                new Business { Id = 1, Email = "business1@test.com", Address="2613 35th Ave W", Bin = "473234", Password="password"},
                new Business { Id = 2, Email = "business2@test.com", Address="123 Number Rd", Bin =  "1523546", Password="Password1"}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "GetLimitedTransactionsByUserId_Database")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {
                context.Transactions.AddRange(transactions);
                context.Users.AddRange(users);
                context.Businesses.AddRange(businesses);
                context.SaveChanges();
            }

            using (var context = new WizardingBankDbContext(options))
            {
                var service = new TransactionServices(context);

                // Act
                var result = service.GetLimitedTransactionsByUserId(1);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<object>>(result);
                Assert.Equal(2, result.Count);
            }

        }

        [Fact]
        public void TestWalletToAccount()
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

            var account = new List<Account>
            {
                new Account {Id = 1, UserId = 1, Balance = 100},
                new Account {Id = 2, UserId = 2, Balance = 200}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "WalletToAccount_Database")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {
                context.Transactions.AddRange(transactions);
                context.Users.AddRange(users);
                context.Accounts.AddRange(account);
                context.SaveChanges();
            }

            using (var context = new WizardingBankDbContext(options))
            {
                var service = new TransactionServices(context);

                // Act
                var result = service.walletToAccount(new Transaction { Id = 300, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1, RecipientId = 1, Description = "Transaction 1", AccountId = 1, SenderType = false });

                // Assert
                Assert.NotNull(result);
                Assert.IsType<Transaction>(result);
                Assert.Equal(100, users[0].Wallet);
            }

        }
        [Fact]
        public void TestWalletToAccount_BusinessTransaction()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                // new Transaction { Id = 300, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1, RecipientId = 1, Description = "Transaction 1", CardId = 1 },
                new Transaction { Id = 3, Amount = 75, CreatedAt = new DateTime(2022, 3, 2), SenderId = 2, RecipientId = 2, Description = "Transaction 2", CardId = 2 }
            };

            var businesses = new List<Business>
            {
                new Business { Id = 1, Email = "user1@test.com", Password = "Password1", Wallet = 100, Bin = "1", Address = "123 st"},
                new Business { Id = 2, Email = "user2@test.com", Password = "Password2", Wallet = 200, Bin = "1", Address = "321 st"},
            };

            var account = new List<Account>
            {
                new Account {Id = 4, UserId = 1, Balance = 100},
                new Account {Id = 5, UserId = 2, Balance = 200}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "WalletToAccount_Database")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {
                context.Transactions.AddRange(transactions);
                context.Businesses.AddRange(businesses);
                context.Accounts.AddRange(account);
                context.SaveChanges();
            }

            using (var context = new WizardingBankDbContext(options))
            {
                var service = new TransactionServices(context);

                // Act
                var result = service.walletToAccount(new Transaction { Id = 305, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1, RecipientId = 1, Description = "Transaction 1", AccountId = 4, SenderType = true });

                // Assert
                Assert.NotNull(result);
                Assert.IsType<Transaction>(result);
                Assert.Equal(100, businesses[0].Wallet);
            }

        }
        [Fact]
        public void TestWalletToCard_UserTest()
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

            var cards = new List<Card>
            {
                new Card {Id = 1, UserId = 1, CardNumber = 12345612345, Balance = 100, Cvv = 123},
                new Card {Id = 2, UserId = 2, CardNumber = 23456712334, Balance = 200, Cvv = 456}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "WalletToCard_Database")
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
                var result = service.walletToCard(new Transaction { Id = 300, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1, RecipientId = 1, Description = "Transaction 1", CardId = 1 });

                // Assert
                Assert.NotNull(result);
                Assert.IsType<Transaction>(result);
                // Assert.Equal(120, users[0].Wallet);
            }

        }

        [Fact]
        public void TestWalletToCard_BusinessTest()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                // new Transaction { Id = 300, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1, RecipientId = 1, Description = "Transaction 1", CardId = 1 },
                new Transaction { Id = 5, Amount = 75, CreatedAt = new DateTime(2022, 3, 2), SenderId = 2, RecipientId = 2, Description = "Transaction 2", CardId = 2 }
            };

            var businesses = new List<Business>
            {

                new Business { Id = 3, Email = "user1@test.com", Password = "Password1", Wallet = 100, Address="524 Ave", Bin="31"},
                new Business { Id = 7, Email = "user2@test.com", Password = "Password2", Wallet = 200, Address="123 C# Rd", Bin="304957-132908"},
            };

            var cards = new List<Card>
            {
                new Card {Id = 12, BusinessId = 3, CardNumber = 12345612345, Balance = 100, Cvv = 123},
                new Card {Id = 13, BusinessId = 7, CardNumber = 23456712334, Balance = 200, Cvv = 456}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "WalletToCard_Database")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {
                context.Transactions.AddRange(transactions);
                context.Businesses.AddRange(businesses);
                context.Cards.AddRange(cards);
                context.SaveChanges();
            }

            using (var context = new WizardingBankDbContext(options))
            {
                var service = new TransactionServices(context);

                // Act
                var result = service.walletToCard(new Transaction { Id = 320, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 7, RecipientId = 1, Description = "Transaction 1", CardId = 1, SenderType = true });

                // Assert
                Assert.NotNull(result);
                Assert.IsType<Transaction>(result);
                // Assert.Equal(120, users[0].Wallet);
            }

        }

        [Fact]
        public void UserToUser_TransactionWithValidData_ReturnsTransaction_BusinessesToBusiness()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 5, Amount = 75, CreatedAt = new DateTime(2022, 3, 2), SenderId = 2, RecipientId = 2, Description = "Transaction 2", CardId = 2 }
            };

            var businesses = new List<Business>
            {

                new Business { Id = 3, Email = "user1@test.com", Password = "Password1", Wallet = 100, Address="524 Ave", Bin="31"},
                new Business { Id = 7, Email = "user2@test.com", Password = "Password2", Wallet = 200, Address="123 C# Rd", Bin="304957-132908"},
            };

            var users = new List<User>
            {
                new User {Id = 122, Wallet = 143, Email = "ga@ga.com", Password = "Password" },
                new User {Id = 133, Wallet = 234, Email = "test@testing.com", Password = "Password1"}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "UserToUser_Database")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {
                context.Transactions.AddRange(transactions);
                context.Businesses.AddRange(businesses);
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            using (var context = new WizardingBankDbContext(options))
            {
                var service = new TransactionServices(context);

                // Act
                var result = service.userToUser(new Transaction { Id = 420, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 7, RecipientId = 3, Description = "Transaction 1", CardId = 1, SenderType = true, RecpientType = true });

                // Assert
                Assert.NotNull(result);
                Assert.IsType<Transaction>(result);
            }
        }

        [Fact]
        public void UserToUser_TransactionWithValidData_ReturnsTransaction_BusinessToUser()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 50, Amount = 75, CreatedAt = new DateTime(2022, 3, 2), SenderId = 2, RecipientId = 2, Description = "Transaction 2", CardId = 2 }
            };

            var businesses = new List<Business>
            {

                new Business { Id = 13, Email = "user1@test.com", Password = "Password1", Wallet = 100, Address="524 Ave", Bin="31"},
                new Business { Id = 17, Email = "user2@test.com", Password = "Password2", Wallet = 200, Address="123 C# Rd", Bin="304957-132908"},
            };

            var users = new List<User>
            {
                new User {Id = 1220, Wallet = 143, Email = "ga@ga.com", Password = "Password" },
                new User {Id = 1330, Wallet = 234, Email = "test@testing.com", Password = "Password1"}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "UserToUser_Database")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {
                context.Transactions.AddRange(transactions);
                context.Businesses.AddRange(businesses);
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            using (var context = new WizardingBankDbContext(options))
            {
                var service = new TransactionServices(context);

                // Act
                var result = service.userToUser(new Transaction { Id = 4200, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 1220, RecipientId = 13, Description = "Transaction 1", CardId = 1, SenderType = false, RecpientType = true });

                // Assert
                Assert.NotNull(result);
                Assert.IsType<Transaction>(result);
            }
        }


        [Fact]
        public void UserToUser_TransactionWithValidData_ReturnsTransaction_UserToBusiness()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 505, Amount = 75, CreatedAt = new DateTime(2022, 3, 2), SenderId = 2, RecipientId = 2, Description = "Transaction 2", CardId = 2 }
            };

            var businesses = new List<Business>
            {

                new Business { Id = 131, Email = "user1@test.com", Password = "Password1", Wallet = 100, Address="524 Ave", Bin="31"},
                new Business { Id = 171, Email = "user2@test.com", Password = "Password2", Wallet = 200, Address="123 C# Rd", Bin="304957-132908"},
            };

            var users = new List<User>
            {
                new User {Id = 12201, Wallet = 143, Email = "ga@ga.com", Password = "Password" },
                new User {Id = 13301, Wallet = 234, Email = "test@testing.com", Password = "Password1"}
            };

            var options = new DbContextOptionsBuilder<WizardingBankDbContext>()
                .UseInMemoryDatabase(databaseName: "UserToBusiness_Database")
                .Options;

            using (var context = new WizardingBankDbContext(options))
            {
                context.Transactions.AddRange(transactions);
                context.Businesses.AddRange(businesses);
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            using (var context = new WizardingBankDbContext(options))
            {
                var service = new TransactionServices(context);

                // Act
                var result = service.userToUser(new Transaction { Id = 42000, Amount = 20, CreatedAt = new DateTime(2022, 3, 1), SenderId = 131, RecipientId = 13301, Description = "Transaction 1", CardId = 1, SenderType = true, RecpientType = false });

                // Assert
                Assert.NotNull(result);
                Assert.IsType<Transaction>(result);
            }
        }
    }
}



