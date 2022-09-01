using Chat.Common;
using Chat.Data;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Application;
using Chat.Infrastructure;
using Chat.Infrastructure.Common;
using Chat.Infrastructure.Identity;

namespace Chat.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public async Task GetUsersAsyncWithoutExclude()
        {
            // Arrange
            var mockDb = new Mock<ChatDbContext>();
            var mockLogger = new Mock<ILogger<UserService>>();
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            Guid guid3 = Guid.NewGuid();
            var user1 = new ApplicationUser { Id = guid1.ToString(), UserName = "user1" };
            var user2 = new ApplicationUser { Id = guid2.ToString(), UserName = "user2" };
            var user3 = new ApplicationUser { Id = guid3.ToString(), UserName = "user3" };
            var users = new List<ApplicationUser> { user1, user2, user3 };
            var mockUsers = users.AsQueryable().BuildMockDbSet();
            mockDb.Setup(x => x.Users).Returns(mockUsers.Object);

            var userService = new UserService(mockLogger.Object, mockDb.Object);

            // Act
            GetUsersResult result = await userService.GetUsersAsync();

            // Assert
            Assert.AreEqual(EDbQueryStatus.Success, result.Status);
            Assert.AreEqual(users.Count, result.Data.Count);
            Assert.IsTrue(result.Data.Any(x => x.UserName == user1.UserName));
        }
        
        [TestMethod]
        public async Task GetUsersAsyncWithExclude()
        {
            // Arrange
            var mockDb = new Mock<ChatDbContext>();
            var mockLogger = new Mock<ILogger<UserService>>();
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            Guid guid3 = Guid.NewGuid();
            var user1 = new ApplicationUser { Id = guid1.ToString(), UserName = "user1" };
            var user2 = new ApplicationUser { Id = guid2.ToString(), UserName = "user2" };
            var user3 = new ApplicationUser { Id = guid3.ToString(), UserName = "user3" };
            var users = new List<ApplicationUser> { user1, user2, user3 };
            var mockUsers = users.AsQueryable().BuildMockDbSet();
            mockDb.Setup(x => x.Users).Returns(mockUsers.Object);

            var userService = new UserService(mockLogger.Object, mockDb.Object);

            // Act
            GetUsersResult result = await userService.GetUsersAsync(user1.UserName);

            // Assert
            Assert.AreEqual(EDbQueryStatus.Success, result.Status);
            Assert.AreEqual(users.Count - 1, result.Data.Count);
            Assert.IsFalse(result.Data.Any(x => x.UserName == user1.UserName));
        }
    }
}
