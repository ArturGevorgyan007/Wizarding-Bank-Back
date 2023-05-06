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
    public class UsersTests
    {
        User user = new User
        {
            Id = 1,
            FullName = "person name",
            Password = "password",
            Address = "123 street",
            Email = "email@email.com"
        };
        User user2 = new User
        {
            Id = 2,
            FullName = "Jason Doe",
            Password = "password2",
            Address = "1234 street",
            Email = "JasonDoe@email.com"
        };
        User updateUser = new User
        {
            Id = 1,
            FullName = "person name new",
            Password = "password",
            Address = "123  c# street",
            Email = "email@email.com"
        };

        [Fact]
        public void updateUserInfo()
        {
            var userList = new List<User>{
                user
            };
            var userQueryable = userList.AsQueryable();
            var userDbSetMock = new Mock<DbSet<User>>();
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Provider).Returns(userQueryable.Provider);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Expression).Returns(userQueryable.Expression);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.ElementType).Returns(userQueryable.ElementType);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(userQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Users).Returns(userDbSetMock.Object);

            var userService = new UserServices(mockContext.Object);
            var result = userService.UpdateUser(updateUser);
            userDbSetMock.Setup(m => m.Update(It.IsAny<User>())).Verifiable();

            //userDbSetMock.Verify(m => m.Update(It.IsAny<User>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.Equal(1, result.Id);
            Assert.Equal("person name new", result.FullName);
            Assert.Equal("password", result.Password);
            Assert.Equal("123  c# street", result.Address);
            Assert.Equal("email@email.com", result.Email);
        }

        [Fact]
        public void getPersonalUserById()
        {
            var userList = new List<User>{
                user,
                user2
            };
            var userQueryable = userList.AsQueryable();
            var userDbSetMock = new Mock<DbSet<User>>();
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Provider).Returns(userQueryable.Provider);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Expression).Returns(userQueryable.Expression);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.ElementType).Returns(userQueryable.ElementType);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(userQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Users).Returns(userDbSetMock.Object);

            var userService = new UserServices(mockContext.Object);
            int tempValueID = 2;
            var result = userService.GetUser(tempValueID);

            Assert.Equal(2, result.Id);
            Assert.Equal("Jason Doe", result.FullName);
            Assert.Equal("password2", result.Password);
            Assert.Equal("1234 street", result.Address);
            Assert.Equal("JasonDoe@email.com", result.Email);
        }

        [Fact]
        public void updateWallet()
        {
            var user = new User
            {
                Id = 1,
                FullName = "person name",
                Password = "password",
                Address = "123 street",
                Email = "email@email.com",
                Wallet = 100.0m
            };

            var userQueryable = new List<User> { user }.AsQueryable();
            var userDbSetMock = new Mock<DbSet<User>>();
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Provider).Returns(userQueryable.Provider);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Expression).Returns(userQueryable.Expression);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.ElementType).Returns(userQueryable.ElementType);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(userQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Users).Returns(userDbSetMock.Object);

            var userService = new UserServices(mockContext.Object);
            decimal amount = 50.0m;
            var result = userService.UpdateWallet(user.Id, amount);

            Assert.Equal(150.0m, result.Wallet);
        }

        [Fact]
        public void CreateUser_ShouldReturnCreatedUser()
        {
            // Arrange
            var newUser = new List<User>
            {new User {
                FullName = "New User",
                Password = "password",
                Address = "123 street",
                Email = "newuser@example.com"}
            };
            var userQueryable = new List<User> { user }.AsQueryable();
            var userDbSetMock = new Mock<DbSet<User>>();
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Provider).Returns(userQueryable.Provider);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Expression).Returns(userQueryable.Expression);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.ElementType).Returns(userQueryable.ElementType);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(userQueryable.GetEnumerator);

            userDbSetMock.Setup(x => x.Add(It.IsAny<User>())).Callback<User>((user) =>
            {
                // Set the ID of the added user to a fixed value for testing purposes
                user.Id = 42;
            });

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Users).Returns(userDbSetMock.Object);

            var userService = new UserServices(mockContext.Object);

            // Act
            var createdUser = userService.CreateUser(newUser[0]);

            // Assert
            userDbSetMock.Verify(m => m.Add(It.IsAny<User>()), Times.Never());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.Equal(0, createdUser.Id);
            Assert.Equal(newUser[0].FullName, createdUser.FullName);
            Assert.Equal(newUser[0].Password, createdUser.Password);
            Assert.Equal(newUser[0].Address, createdUser.Address);
            Assert.Equal(newUser[0].Email, createdUser.Email);
        }
        [Fact]
        public void deleteUser()
        {
            var user = new User
            {
                Id = 1,
                FullName = "person name",
                Password = "password",
                Address = "123 street",
                Email = "email@email.com"
            };

            var userList = new List<User>{
                user
            };
            var userQueryable = userList.AsQueryable();
            var userDbSetMock = new Mock<DbSet<User>>();
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Provider).Returns(userQueryable.Provider);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.Expression).Returns(userQueryable.Expression);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.ElementType).Returns(userQueryable.ElementType);
            userDbSetMock.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(userQueryable.GetEnumerator);

            var mockContext = new Mock<WizardingBankDbContext>();
            mockContext.Setup(m => m.Users).Returns(userDbSetMock.Object);

            var userService = new UserServices(mockContext.Object);
            var result = userService.DeleteUser(user);

            userDbSetMock.Verify(m => m.Remove(It.IsAny<User>()), Times.Never());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.NotNull(userService.GetUser(user.Id));
        }
    }


}