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
    public class LoanServicesTests
    {
        Loan loan = new Loan
        {
            Id = 1,
            BusinessId = 1,
            Amount = (decimal)10000,
            InterestRate = (decimal)0.05,
            DateLoaned = DateTime.Now,
            LoanPaid = DateTime.Now,
            MonthlyPay = (decimal)100,
            AmountPaid = (decimal)200
        };
        Loan loan2 = new Loan
        {
            Id = 2,
            BusinessId = 1,
            Amount = (decimal)20000,
            InterestRate = (decimal)0.07,
            DateLoaned = DateTime.Now,
            LoanPaid = DateTime.Now,
            MonthlyPay = (decimal)100,
            AmountPaid = (decimal)10000
        };
        Loan loan3 = new Loan
        {
            Id = 3,
            BusinessId = 1,
            Amount = (decimal)10000,
            InterestRate = (decimal)0.07,
            DateLoaned = DateTime.Now,
            LoanPaid = DateTime.Now,
            MonthlyPay = (decimal)100,
            AmountPaid = (decimal)5000
        };

        [Fact]
        public void CreateBusinessLoan_ShouldCreateLoanAndAddToDbContext()
        {
            // Arrange
            var business = new List<Business>
            {
                new Business{
                Id = 1,
                Wallet = 5000
                },
            };
            var loans = new List<Loan>
            {
                loan,
                loan2,
                loan3
            };

            var loanQueryable = loans.AsQueryable();
            var businessQueryable = business.AsQueryable();

            var loansDbSetMock = new Mock<DbSet<Loan>>();
            var businessDbSetMock = new Mock<DbSet<Business>>();

            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(businessQueryable.Provider);
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(businessQueryable.Expression);
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(businessQueryable.ElementType);
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(businessQueryable.GetEnumerator);

            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.Provider).Returns(loanQueryable.Provider);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.Expression).Returns(loanQueryable.Expression);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.ElementType).Returns(loanQueryable.ElementType);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.GetEnumerator()).Returns(loanQueryable.GetEnumerator);

            var contextMock = new Mock<WizardingBankDbContext>();
            contextMock.Setup(x => x.Businesses).Returns(businessDbSetMock.Object);
            contextMock.Setup(x => x.Loans).Returns(loansDbSetMock.Object);

            var loanServices = new LoanServices(contextMock.Object);

            // Act
            Loan result = loanServices.CreateBusinessLoan(this.loan);

            // Assert
            contextMock.Verify(x => x.Loans.Add(It.IsAny<Loan>()), Times.Once);
            contextMock.Verify(x => x.SaveChanges(), Times.Once);

            Assert.Equal(10000, result.Amount);
            Assert.NotNull(result.LoanPaid);
            Assert.True(business[0].Wallet >= 15000);
        }

        [Fact]
        public void GetAllBusinessLoan_ShouldReturnListWithUnpaidLoans()
        {
            // Arrange
            var loans = new List<Loan>
            {
                loan,
                loan2,
                loan3
            };
            var loanQueryable = loans.AsQueryable();

            var loansDbSetMock = new Mock<DbSet<Loan>>();
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.Provider).Returns(loanQueryable.Provider);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.Expression).Returns(loanQueryable.Expression);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.ElementType).Returns(loanQueryable.ElementType);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.GetEnumerator()).Returns(loanQueryable.GetEnumerator);

            var contextMock = new Mock<WizardingBankDbContext>();
            contextMock.Setup(x => x.Loans).Returns(loansDbSetMock.Object);

            var loanServices = new LoanServices(contextMock.Object);

            // Act
            var result = loanServices.GetAllBusinessLoan(1);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(10000, result[0].Amount);
            Assert.Equal(5000, result[2].AmountPaid);
            Assert.Equal(0.05m, result[0].InterestRate);
            // Assert.Equal(DateTime.Now, result[0].DateLoaned);

            Assert.Equal(2, result[1].Id);
            Assert.Equal(20000, result[1].Amount);
            Assert.Equal(10000, result[1].AmountPaid);
            Assert.Equal(0.07m, result[1].InterestRate);
            // Assert.Equal(DateTime.Now.AddMonths(-1), result[1].DateLoaned);
            // Assert.Equal(DateTime.Now.AddMonths(-1), result[1].LoanPaid);
        }
        [Fact]
        public void PayLoan_FullPayment_Success()
        {
            // Arrange
            var business = new List<Business>
            {
                new Business{
                Id = 1,
                Wallet = 2000
                },
            };
            var loans = new List<Loan>
            {
                loan
            };
            var loanQueryable = loans.AsQueryable();
            var businessQueryable = business.AsQueryable();
            var loansDbSetMock = new Mock<DbSet<Loan>>();
            // loansDbSetMock.Setup(x => x.SingleOrDefault(It.IsAny<Func<Loan, bool>>()))
            //     .Returns(loan);

            var businessDbSetMock = new Mock<DbSet<Business>>();
            var contextMock = new Mock<WizardingBankDbContext>();

            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(businessQueryable.Provider);
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(businessQueryable.Expression);
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(businessQueryable.ElementType);
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(businessQueryable.GetEnumerator);

            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.Provider).Returns(loanQueryable.Provider);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.Expression).Returns(loanQueryable.Expression);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.ElementType).Returns(loanQueryable.ElementType);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.GetEnumerator()).Returns(loanQueryable.GetEnumerator);
            contextMock.Setup(x => x.Loans).Returns(loansDbSetMock.Object);
            contextMock.Setup(x => x.Businesses).Returns(businessDbSetMock.Object);
            var controller = new LoanServices(contextMock.Object);

            // Act
            var result = controller.PayLoan(1, 1000, 1000);

            // Assert
            Assert.Equal(1200, result.AmountPaid);
            Assert.Equal(DateTime.Today, result.LoanPaid.Value.Date);
            Assert.Equal(1000, business[0].Wallet);
            // loansDbSetMock.Verify(x => x.Update(loan), Times.Once);
            // contextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void PayLoan_PartialPayment_Success()
        {
            // Arrange
            var business = new List<Business>
            {
                new Business{
                Id = 1,
                Wallet = 2000
                },
            };
            var loans = new List<Loan>
            {
                loan,
            };
            var loanQueryable = loans.AsQueryable();
            var loansDbSetMock = new Mock<DbSet<Loan>>();

            var businessQueryable = business.AsQueryable();
            // loansDbSetMock.Setup(x => x.SingleOrDefault(It.IsAny<Func<Loan, bool>>()))
            //     .Returns(loan);

            var businessDbSetMock = new Mock<DbSet<Business>>();
            var mockDbSet = new Mock<DbSet<Loan>>();
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(businessQueryable.Provider);
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(businessQueryable.Expression);
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(businessQueryable.ElementType);
            businessDbSetMock.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(businessQueryable.GetEnumerator);

            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.Provider).Returns(loanQueryable.Provider);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.Expression).Returns(loanQueryable.Expression);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.ElementType).Returns(loanQueryable.ElementType);
            loansDbSetMock.As<IQueryable<Loan>>().Setup(m => m.GetEnumerator()).Returns(loanQueryable.GetEnumerator);
            // mockDbSet.Setup(x => x.SingleOrDefault(It.IsAny<Func<Loan, bool>>()))
            //     .Returns(loan);
            var contextMock = new Mock<WizardingBankDbContext>();
            contextMock.Setup(x => x.Loans).Returns(loansDbSetMock.Object);
            contextMock.Setup(x => x.Businesses).Returns(businessDbSetMock.Object);
            var controller = new LoanServices(contextMock.Object);

            // Act
            var result = controller.PayLoan(1, 500, 1000);

            // Assert
            Assert.Equal(700, result.AmountPaid);
            Assert.Equal(DateTime.Today.AddMonths(1), result.LoanPaid.Value.Date);
            Assert.Equal(1000, business[0].Wallet);
            // mockDbSet.Verify(x => x.Update(loan), Times.Once);
            // contextMock.Verify(x => x.SaveChanges(), Times.Once);
        }
    }

}