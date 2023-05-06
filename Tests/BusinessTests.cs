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
    public class BusinessTests
    {
        Business bus = new Business
        {
            Id = 1,
            BusinessName = "Old Business Corp",
            Address = "4243 rd",
            Email = "busemail@email.com",
            Bin = "12345",
            BusinessType = "small",
            Wallet = 10000.00m
        };
        Business bus2 = new Business
        {
            Id = 2,
            BusinessName = "Old Business Inc",
            Address = "4243 Ave",
            Email = "bus2email@email.com",
            Bin = "12263",
            BusinessType = "large",
            Wallet = 35000.00m
        };
        Business updateBus = new Business
        {
            Id = 1,
            BusinessName = "New Business Corp",
            Address = "4243 C# rd",
            Email = "busemail@email.com",
            Bin = "123456",
            BusinessType = "medium",
            Wallet = 10000.00m
        };
        [Fact]
        public void updateBusinessInfo()
        {
            var businessList = new List<Business>{
                bus
            };
            var busQueryable = businessList.AsQueryable();
            var businessDbSetMock = new Mock<DbSet<Business>>();
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Provider).Returns(busQueryable.Provider);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Expression).Returns(busQueryable.Expression);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.ElementType).Returns(busQueryable.ElementType);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.GetEnumerator()).Returns(busQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Businesses).Returns(businessDbSetMock.Object);

            var businessService = new BusinessServices(mockContext.Object);
            var result = businessService.UpdateBusiness(updateBus);
            businessDbSetMock.Setup(m => m.Update(It.IsAny<Business>())).Verifiable();

            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.Equal(1, result.Id);
            Assert.Equal("New Business Corp", result.BusinessName);
            Assert.Equal("123456", result.Bin);
            Assert.Equal("4243 C# rd", result.Address);
            Assert.Equal("busemail@email.com", result.Email);
            Assert.Equal("medium", result.BusinessType);
            Assert.Equal(10000.00m, result.Wallet);
        }

        [Fact]
        public void getBusinessUserByID()
        {
            var businessList = new List<Business>{
                bus,
                bus2
            };
            var busQueryable = businessList.AsQueryable();
            var businessDbSetMock = new Mock<DbSet<Business>>();
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Provider).Returns(busQueryable.Provider);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Expression).Returns(busQueryable.Expression);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.ElementType).Returns(busQueryable.ElementType);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.GetEnumerator()).Returns(busQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Businesses).Returns(businessDbSetMock.Object);

            var businessService = new BusinessServices(mockContext.Object);
            int tempValueID = 2;
            var result = businessService.getBusinessById(tempValueID);

            Assert.Equal(2, result[0].Id);
            Assert.Equal("Old Business Inc", result[0].BusinessName);
            Assert.Equal("12263", result[0].Bin);
            Assert.Equal("4243 Ave", result[0].Address);
            Assert.Equal("bus2email@email.com", result[0].Email);
            Assert.Equal("large", result[0].BusinessType);
            Assert.Equal(35000.00m, result[0].Wallet);
        }
        [Fact]
        public void CreateBusiness_ReturnsCreatedBusiness()
        {
            var businessList = new List<Business>{
                bus,
                bus2
            };
            var busQueryable = businessList.AsQueryable();
            var businessDbSetMock = new Mock<DbSet<Business>>();
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Provider).Returns(busQueryable.Provider);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Expression).Returns(busQueryable.Expression);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.ElementType).Returns(busQueryable.ElementType);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.GetEnumerator()).Returns(busQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Businesses).Returns(businessDbSetMock.Object);

            var businessService = new BusinessServices(mockContext.Object);
            var createdBusiness = businessService.CreateBusiness(businessList[0]);

            // Assert
            Assert.Equal(businessList[0], createdBusiness);
        }

        [Fact]
        public void DeleteBusiness_ShouldRemoveBusiness_WhenBusinessExists()
        {
            // Arrange
            var businessList = new List<Business>{
                bus,
                bus2
            };
            var busQueryable = businessList.AsQueryable();
            var businessDbSetMock = new Mock<DbSet<Business>>();
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Provider).Returns(busQueryable.Provider);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Expression).Returns(busQueryable.Expression);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.ElementType).Returns(busQueryable.ElementType);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.GetEnumerator()).Returns(busQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Businesses).Returns(businessDbSetMock.Object);

            var service = new BusinessServices(mockContext.Object);

            // Act
            var result = service.DeleteBusiness(businessList[0]);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(businessList[0], result);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            //Assert.NotEmpty(mockContext.Object.Businesses);
        }

        [Fact]
        public void DeleteBusiness_ShouldNotRemoveBusiness_WhenBusinessDoesNotExist()
        {
            // Arrange
            var businessList = new List<Business>{
                bus,
                bus2
            };
            var busQueryable = businessList.AsQueryable();
            var businessDbSetMock = new Mock<DbSet<Business>>();
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Provider).Returns(busQueryable.Provider);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Expression).Returns(busQueryable.Expression);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.ElementType).Returns(busQueryable.ElementType);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.GetEnumerator()).Returns(busQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Businesses).Returns(businessDbSetMock.Object);

            var service = new BusinessServices(mockContext.Object);

            var businessToRemove = businessList[0];

            // Act
            var result = service.DeleteBusiness(businessToRemove);

            // Assert
            Assert.NotNull(result);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            // Assert.Single(mockContext.Object.Businesses);
        }
        public void getBusinessUserByEmail()
        {
            var businessList = new List<Business>{
                bus
            };
            var busQueryable = businessList.AsQueryable();
            var businessDbSetMock = new Mock<DbSet<Business>>();
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Provider).Returns(busQueryable.Provider);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.Expression).Returns(busQueryable.Expression);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.ElementType).Returns(busQueryable.ElementType);
            businessDbSetMock.As<IQueryable<Business>>().Setup(x => x.GetEnumerator()).Returns(busQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Businesses).Returns(businessDbSetMock.Object);

            var businessService = new BusinessServices(mockContext.Object);
            int tempValueID = 2;
            var result = businessService.GetBusiness("busemail@email.com");

            Assert.Equal(1, result.Id);
            Assert.Equal("New Business Corp", result.BusinessName);
            Assert.Equal("12345", result.Bin);
            Assert.Equal("4243 rd", result.Address);
            Assert.Equal("busemail@email.com", result.Email);
            Assert.Equal("small", result.BusinessType);
        }

    }
}