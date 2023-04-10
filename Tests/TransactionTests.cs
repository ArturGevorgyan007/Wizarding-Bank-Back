using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using DataAccess.Entities;
using Services;

namespace Tests
{
    public class TransactionTests
    {


        private readonly Mock<WizardingBankDbContext> _contextMock;

        public TransactionTests()
        {
            _contextMock = new Mock<WizardingBankDbContext>();
        }

        [Fact]
        public void GetAllTransactions_ShouldReturnAllTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>()
            {
                new Transaction() { Id = 1, Amount = 100, CreatedAt = DateTime.Now },
                new Transaction() { Id = 2, Amount = 200, CreatedAt = DateTime.Now.AddDays(-1) }
            };
            var transactionQueryable = transactions.AsQueryable();
            var transactionsDbSetMock = new Mock<DbSet<Transaction>>();
            transactionsDbSetMock.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(transactionQueryable.Provider);
            transactionsDbSetMock.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(transactionQueryable.Expression);
            transactionsDbSetMock.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(transactionQueryable.ElementType);
            transactionsDbSetMock.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(transactionQueryable.GetEnumerator);

            _contextMock.Setup(c => c.Transactions).Returns(transactionsDbSetMock.Object);

            var transactionServices = new TransactionServices(_contextMock.Object);

            // Act
            var result = transactionServices.GetAllTransactions();

            // Assert
            Assert.Equal(transactions.Count, result.Count);
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
    }
}