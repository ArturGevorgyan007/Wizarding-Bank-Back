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
    public class AccountsTest
    {
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
        [Fact]
        public void GetAllAccounts_ReturnsListOfAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { Id = 1, BusinessId = 1, UserId = 1 },
                new Account { Id = 2, BusinessId = 2, UserId = 2 },
                new Account { Id = 3, BusinessId = 3, UserId = 3 }
            };
            var accQueryable = accounts.AsQueryable();
            var _mockContext = new Mock<WizardingBankDbContext>();
            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);


            var service = new AccountServices(_mockContext.Object);
            _mockContext.Setup(c => c.Accounts).Returns(accountsDbSetMock.Object);

            // Act
            var result = service.getAllAccounts();

            // Assert
            Assert.Equal(accounts, result);
            // _mockContext.Verify(c => c.Accounts.ToList(), Times.Once);
        }

        [Fact]
        public void GetAccounts_ReturnsListOfAccountsForGivenId()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { Id = 1, BusinessId = 1, UserId = 1 },
                new Account { Id = 2, BusinessId = 2, UserId = 1 },
                new Account { Id = 3, BusinessId = 3, UserId = 2 }
            };
            var _mockContext = new Mock<WizardingBankDbContext>();
            var accQueryable = accounts.AsQueryable();
            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);


            var service = new AccountServices(_mockContext.Object);
            _mockContext.Setup(c => c.Accounts).Returns(accountsDbSetMock.Object);

            // Act
            var result = service.getAccounts(1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.True(result.All(a => a.UserId == 1));
            // _mockContext.Verify(c => c.Accounts.ToList(), Times.Once);
        }

        [Fact]
        public void UpdateAccount_UpdatesAccountAndReturnsUpdatedAccount()
        {
            var _mockContext = new Mock<WizardingBankDbContext>();

            // Arrange
            var accounts = new List<Account> { new Account { Id = 1, BusinessId = 1, UserId = 1 } };
            var service = new AccountServices(_mockContext.Object);
            var accQueryable = accounts.AsQueryable();
            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);

            _mockContext.Setup(c => c.Accounts).Returns(accountsDbSetMock.Object);
            _mockContext.Setup(c => c.Update(accounts)).Verifiable();

            _mockContext.Setup(c => c.SaveChanges()).Verifiable();

            // Act
            var result = service.updateAccount(accounts[0]);

            // Assert
            Assert.Equal(accounts[0], result);
            // _mockContext.Verify();
        }
        [Fact]
        public void UpdateAccountBalance_UpdatesAccountBalance()
        {
            // Arrange
            var _mockContext = new Mock<WizardingBankDbContext>();

            var accounts = new List<Account> { new Account { Id = 1, Balance = 100 } };
            var accQueryable = accounts.AsQueryable();
            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);

            _mockContext.Setup(c => c.Accounts).Returns(accountsDbSetMock.Object);

            var accountServices = new AccountServices(_mockContext.Object);

            // Act
            var result = accountServices.updateAccountBalance(1, 50);

            // Assert
            Assert.Equal(150, result.Balance);
        }

        [Fact]
        public void GetAccountById_ReturnsAccountIfExists()
        {
            // Arrange
            var _mockContext = new Mock<WizardingBankDbContext>();

            var accounts = new List<Account> { new Account { Id = 1, Balance = 100 } };
            var accQueryable = accounts.AsQueryable();
            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);

            _mockContext.Setup(c => c.Accounts).Returns(accountsDbSetMock.Object);

            var accountServices = new AccountServices(_mockContext.Object);

            // Act
            var result = accountServices.getAccountById(1);

            // Assert
            Assert.Equal(accounts[0], result);
        }

        [Fact]
        public void GetAccountById_ReturnsNullIfAccountDoesNotExist()
        {
            // Arrange
            var _mockContext = new Mock<WizardingBankDbContext>();

            var accounts = new List<Account> { new Account { Id = 1, Balance = 100 } };
            var accQueryable = accounts.AsQueryable();
            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);

            _mockContext.Setup(c => c.Accounts).Returns(accountsDbSetMock.Object);

            var accountServices = new AccountServices(_mockContext.Object);


            // Act
            var result = accountServices.getAccountById(1);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteAccount_DeletesAccountIfItExists()
        {
            // Arrange
            var account1 = new Account { Id = 1 };
            var account2 = new Account { Id = 2 };

            var _mockContext = new Mock<WizardingBankDbContext>();

            var accounts = new List<Account> { new Account { Id = 1, Balance = 100 }, new Account { Id = 2, Balance = 100 } };
            var accQueryable = accounts.AsQueryable();
            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);

            _mockContext.Setup(c => c.Accounts).Returns(accountsDbSetMock.Object);

            var accountServices = new AccountServices(_mockContext.Object);

            // Act
            var result = accountServices.deleteAccount(1, 1);

            // Assert
            Assert.False(result);
            Assert.DoesNotContain(account1, accountsDbSetMock.Object);
        }

        [Fact]
        public void DeleteAccount_ReturnsFalseIfAccountDoesNotExist()
        {
            // Arrange
            var _mockContext = new Mock<WizardingBankDbContext>();

            var accounts = new List<Account> { };
            var accQueryable = accounts.AsQueryable();
            var accountsDbSetMock = new Mock<DbSet<Account>>();
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accQueryable.Provider);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accQueryable.Expression);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accQueryable.ElementType);
            accountsDbSetMock.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accQueryable.GetEnumerator);

            _mockContext.Setup(c => c.Accounts).Returns(accountsDbSetMock.Object);

            var accountServices = new AccountServices(_mockContext.Object);

            // Act
            var result = accountServices.deleteAccount(1, 1);

            // Assert
            Assert.False(result);
        }
    }
}