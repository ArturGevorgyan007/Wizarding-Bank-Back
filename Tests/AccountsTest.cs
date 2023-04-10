using DataAccess.Entities;
using Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Tests {
    public class AccountsTest {
        Account act1 = new Account
        {
            Id = 1,
            AccountNumber = "1002045896",
            RoutingNumber = "100201",
            UserId = 1,
            Balance = 2000.90m
        };
        Account act2 = new Account
        {
            Id = 2,
            AccountNumber = "1002045897",
            RoutingNumber = "100202",
            BusinessId = 1,
            Balance = 5000.90m
        };

        [Fact]
        public void shouldCreateAccountWithUserIDOne()
        {
            var accList = new List<Account>{
                act1
            };

            var accQueryable = accList.AsQueryable();

            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);

            var contextMock = new Mock<WizardingBankDbContext>();
            contextMock.Setup(x => x.Accounts).Returns(accountsDbSetMock.Object);

            var accountServices = new AccountServices(contextMock.Object);

            var result = accountServices.createAccount(act1);

            Assert.Equal(1, result.Id);
            Assert.Equal("1002045896", result.AccountNumber);
            Assert.Equal("100201", result.RoutingNumber);
            Assert.Equal(1, result.UserId);
            Assert.Equal(2000.90m, result.Balance);
        }

        [Fact]
        public void shouldCreateAccountWithBusinessIDOne()
        {
             var accList = new List<Account>{
                act2
            };

            var accQueryable = accList.AsQueryable();

            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);

            var contextMock = new Mock<WizardingBankDbContext>();
            contextMock.Setup(x => x.Accounts).Returns(accountsDbSetMock.Object);

            var accountServices = new AccountServices(contextMock.Object);

            var result = accountServices.createAccount(act2);

            Assert.Equal(2, result.Id);
            Assert.Equal("1002045897", result.AccountNumber);
            Assert.Equal("100202", result.RoutingNumber);
            Assert.Equal(1, result.BusinessId);
            Assert.Equal(5000.90m, result.Balance);
        }

        // [Fact]
        // public void GetAccounts_ShouldReturnListOfAccountsForUserIdOrBusinessId()
        // {
        //     // Arrange
        //     var mockContext = new Mock<WizardingBankDbContext>();
        //     var accountServices = new AccountServices(mockContext.Object);
        //     var accounts = new List<Account> {
        //         new Account { Id = 1, UserId = 1, BusinessId = null },
        //         new Account { Id = 2, UserId = null, BusinessId = 2 },
        //         new Account { Id = 3, UserId = 1, BusinessId = null }
        //     }.AsQueryable();

        //     var mockSet = new Mock<DbSet<Account>>();
        //     mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(accounts.Provider);
        //     mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(accounts.Expression);
        //     mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(accounts.ElementType);
        //     mockSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(accounts.GetEnumerator());

        //     mockContext.Setup(x => x.Accounts).Returns(mockSet.Object);

        //     // Act
        //     var result1 = accountServices.getAccounts(1);
        //     //var result2 = accountServices.getAccounts(2);

        //     // Assert
        //     Assert.Equal(accounts, result1);
        // }
    }
}