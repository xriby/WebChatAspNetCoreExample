using Chat.Application.Common;
using Chat.Application.Identity;
using Chat.Application.Interfaces;
using Chat.Application.Interfaces.Repositories;
using Chat.Application.Models;
using Chat.Application.ModelsDto;
using Chat.Application.Results;
using Chat.Application.Services;
using Chat.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chat.Tests.Services
{
    [TestClass]
    public class MessageServiceTests
    {
        [TestMethod]
        public async Task AddEmptyMessageAsync()
        {
            // Arrange
            MessageService messageService = MockMessageService();

            MessageDto message = new();
            string user = "user";

            // Act
            AddMessageResult result = await messageService.AddMessageAsync(message, user);

            // Assert
            Assert.AreEqual(EDbQueryStatus.Failure, result.Status);
            Assert.AreEqual("Ошибка. Введите сообщение.", result.ErrorMessage);
        }

        [TestMethod]
        public async Task MaxTextAddMessageAsync()
        {
            MessageService messageService = MockMessageService();
            int moreThanMaxText = ChatConfiguration.MaxTextLength + 1;

            MessageDto message = new() { Text = new('a', moreThanMaxText) };
            string user = "user";

            AddMessageResult result = await messageService.AddMessageAsync(message, user);

            Assert.AreEqual(EDbQueryStatus.Failure, result.Status);
            Assert.IsTrue(result.ErrorMessage.Contains("Ошибка"));
        }

        [TestMethod]
        public async Task EmptyUserAddMessageAsync()
        {
            MessageService messageService = MockMessageService();

            MessageDto message = new() { Text = "Message" };

            AddMessageResult result = await messageService.AddMessageAsync(message, null);

            Assert.AreEqual(EDbQueryStatus.Failure, result.Status);
            Assert.AreEqual("Ошибка. Не задан пользователь.", result.ErrorMessage);
        }

        [TestMethod]
        public async Task AddMessage_Must_Return_NotFoundUser()
        {
            Guid giud1 = Guid.NewGuid();
            Guid giud2 = Guid.NewGuid();
            ApplicationUser user1 = new() { Id = giud1.ToString(), UserName = "user1" };
            ApplicationUser user2 = new() { Id = giud2.ToString(), UserName = "user2" };
            List<ApplicationUser> users = new() { user1, user2 };
            List<Message> messages = new()
            {
                new()
                {
                    MessageId = 1,
                    Text = "Message1",
                    MessageType = EMessageType.Public,
                    CreateDate = DateTime.UtcNow,
                    User = user1
                },
                new()
                {
                    MessageId = 2,
                    Text = "Message2",
                    MessageType = EMessageType.Public,
                    CreateDate = DateTime.UtcNow,
                    User = user2
                }
            };

            Mock<ChatDbContext> mockDb = new();
            Mock<IUserService> mockUserService = new();
            Mock<ILogger<MessageService>> mockLogger = new();
            Mock<IMessageRepository> mockMessageRepository = new();
            Mock<IUserRepository> mockUserRepository = new();

            Mock<DbSet<ApplicationUser>> mockUsers = users.AsQueryable().BuildMockDbSet();
            Mock<DbSet<Message>> mockMessages = messages.AsQueryable().BuildMockDbSet();

            mockDb.Setup(x => x.Users).Returns(mockUsers.Object);
            mockDb.Object.Messages = mockMessages.Object;

            mockMessages.Setup(_ => _.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
                .Callback((Message model, CancellationToken token) => { messages.Add(model); })
                .ReturnsAsync((Message model, CancellationToken token) => null);

            mockUserRepository.Setup(x => x.GetAllQueryable(It.IsAny<CancellationToken>()))
                .Returns(mockUsers.Object);

            MessageService messageService = new(mockLogger.Object,
                mockUserService.Object,
                mockMessageRepository.Object,
                mockUserRepository.Object);

            MessageDto message = new() { Text = "Message" };
            string user = "user";

            AddMessageResult result = await messageService.AddMessageAsync(message, user);

            Assert.AreEqual(EDbQueryStatus.Failure, result.Status);
            Assert.AreEqual("Ошибка. Пользователь не найден.", result.ErrorMessage);
            mockMessages.Verify(x => x.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Never);
            mockDb.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task AddMessageAsync_Must_Return_Success()
        {
            Guid giud1 = Guid.NewGuid();
            Guid giud2 = Guid.NewGuid();
            ApplicationUser user1 = new() { Id = giud1.ToString(), UserName = "user1" };
            ApplicationUser user2 = new() { Id = giud2.ToString(), UserName = "user2" };
            List<ApplicationUser> users = new() { user1, user2 };
            List<Message> messages = new()
            {
                new()
                {
                    MessageId = 1,
                    Text = "Message1",
                    MessageType = EMessageType.Public,
                    CreateDate = DateTime.UtcNow,
                    User = user1
                },
                new()
                {
                    MessageId = 2,
                    Text = "Message2",
                    MessageType = EMessageType.Public,
                    CreateDate = DateTime.UtcNow,
                    User = user2
                }
            };

            Mock<ChatDbContext> mockDb = new();
            Mock<IUserService> mockUserService = new();
            Mock<ILogger<MessageService>> mockLogger = new();
            Mock<IMessageRepository> mockMessageRepository = new();
            Mock<IUserRepository> mockUserRepository = new();

            Mock<DbSet<ApplicationUser>> mockUsers = users.AsQueryable().BuildMockDbSet();
            Mock<DbSet<Message>> mockMessages = messages.AsQueryable().BuildMockDbSet();

            mockDb.Setup(x => x.Users).Returns(mockUsers.Object);
            mockDb.Object.Messages = mockMessages.Object;

            mockMessages.Setup(_ => _.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
                .Callback((Message model, CancellationToken token) => { messages.Add(model); })
                .ReturnsAsync((Message model, CancellationToken token) => null);

            mockUserRepository.Setup(x => x.GetAllQueryable(It.IsAny<CancellationToken>()))
                .Returns(mockUsers.Object);

            MessageService messageService = new(mockLogger.Object,
                mockUserService.Object,
                mockMessageRepository.Object,
                mockUserRepository.Object);

            MessageDto message = new() { Text = "Message" };
            string user = "user1";

            AddMessageResult result = await messageService.AddMessageAsync(message, user);

            Assert.AreEqual(EDbQueryStatus.Success, result.Status);
        }

        [TestMethod]
        public async Task NullUserGetMessageInfoAsync()
        {
            MessageService messageService = MockMessageService();

            try
            {
                MessageInfoResult result = await messageService.GetMessageInfoAsync(null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public async Task GetMessageInfoAsync()
        {
            Guid giud1 = Guid.NewGuid();
            Guid giud2 = Guid.NewGuid();
            Guid giud3 = Guid.NewGuid();
            ApplicationUser user1 = new() { Id = giud1.ToString(), UserName = "user1" };
            ApplicationUser user2 = new() { Id = giud2.ToString(), UserName = "user2" };
            ApplicationUser user3 = new() { Id = giud3.ToString(), UserName = "user3" };
            List<ApplicationUser> users = new() { user1, user2, user3 };
            List<Message> messages = new()
            {
                new()
                {
                    MessageId = 1,
                    Text = "Message1",
                    MessageType = EMessageType.Public,
                    CreateDate = DateTime.UtcNow,
                    User = user1
                },
                new()
                {
                    MessageId = 2,
                    Text = "Message2",
                    MessageType = EMessageType.Public,
                    CreateDate = DateTime.UtcNow,
                    User = user2
                }
            };

            Mock<ChatDbContext> mockDb = new();
            Mock<IUserService> mockUserService = new();
            Mock<ILogger<MessageService>> mockLogger = new();
            Mock<IMessageRepository> mockMessageRepository = new();
            Mock<IUserRepository> mockUserRepository = new();

            Mock<DbSet<ApplicationUser>> mockUsers = users.AsQueryable().BuildMockDbSet();
            Mock<DbSet<Message>> mockMessages = messages.AsQueryable().BuildMockDbSet();

            mockDb.Setup(x => x.Users).Returns(mockUsers.Object);
            mockDb.Object.Messages = mockMessages.Object;

            GetUsersResult usersResult = new() { Status = EDbQueryStatus.Success, Data = users };

            mockUserService.Setup(x => x.GetUsersAsync(user1.UserName)).ReturnsAsync(usersResult);

            mockUserRepository.Setup(x => x.GetAllQueryable(It.IsAny<CancellationToken>()))
                .Returns(mockUsers.Object);
            mockMessageRepository.Setup(x => x.GetAllQueryable(It.IsAny<CancellationToken>()))
                .Returns(mockMessages.Object);

            MessageService messageService = new(mockLogger.Object,
                mockUserService.Object,
                mockMessageRepository.Object,
                mockUserRepository.Object);

            MessageInfoResult result = await messageService.GetMessageInfoAsync(user1.UserName);

            Assert.AreEqual(EDbQueryStatus.Success, result.Status);
            Assert.AreEqual(users.Count, result.Users.Count);
            Assert.AreEqual(messages.Count, result.Messages.Count);
            mockUserService.Verify(x => x.GetUsersAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task NullUserGetPrivateMessageInfoAsync()
        {
            MessageService messageService = MockMessageService();
            string fromUser = "user1";
            string toUser = "user2";

            try
            {
                PrivateMessageInfoResult result = await messageService.GetPrivateMessageInfoAsync(null, toUser);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }

            try
            {
                PrivateMessageInfoResult result = await messageService.GetPrivateMessageInfoAsync(fromUser, null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public async Task GetPrivateMessageInfoAsync()
        {
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            Guid guid3 = Guid.NewGuid();
            ApplicationUser user1 = new() { Id = guid1.ToString(), UserName = "user1" };
            ApplicationUser user2 = new() { Id = guid2.ToString(), UserName = "user2" };
            ApplicationUser user3 = new() { Id = guid3.ToString(), UserName = "user3" };
            List<ApplicationUser> users = new() { user1, user2, user3 };
            // В списке два приватных сообщения м/у user1 и user2
            List<Message> messages = new()
            {
                new()
                {
                    MessageId = 1,
                    Text = "Message1",
                    MessageType = EMessageType.Private,
                    CreateDate = DateTime.UtcNow,
                    User = user1,
                    RecipientId = guid2.ToString()
                },
                new()
                {
                    MessageId = 2,
                    Text = "Message2",
                    MessageType = EMessageType.Private,
                    CreateDate = DateTime.UtcNow,
                    User = user2,
                    RecipientId = guid2.ToString()
                },
                new()
                {
                    MessageId = 3,
                    Text = "Message3",
                    MessageType = EMessageType.Public,
                    CreateDate = DateTime.UtcNow,
                    User = user2
                }
            };

            Mock<ChatDbContext> mockDb = new();
            Mock<IUserService> mockUserService = new();
            Mock<ILogger<MessageService>> mockLogger = new();
            Mock<IMessageRepository> mockMessageRepository = new();
            Mock<IUserRepository> mockUserRepository = new();

            Mock<DbSet<ApplicationUser>> mockUsers = users.AsQueryable().BuildMockDbSet();
            Mock<DbSet<Message>> mockMessages = messages.AsQueryable().BuildMockDbSet();

            mockDb.Setup(x => x.Users).Returns(mockUsers.Object);
            mockDb.Object.Messages = mockMessages.Object;

            GetUsersResult usersResult = new() { Status = EDbQueryStatus.Success, Data = users };

            mockUserService.Setup(x => x.GetUsersAsync(user1.UserName)).ReturnsAsync(usersResult);

            mockUserRepository.Setup(x => x.GetAllQueryable(It.IsAny<CancellationToken>()))
                .Returns(mockUsers.Object);
            mockMessageRepository.Setup(x => x.GetAllQueryable(It.IsAny<CancellationToken>()))
                .Returns(mockMessages.Object);

            MessageService messageService = new(mockLogger.Object,
                mockUserService.Object,
                mockMessageRepository.Object,
                mockUserRepository.Object);

            PrivateMessageInfoResult result = await messageService.GetPrivateMessageInfoAsync(user1.UserName, user2.UserName);

            Assert.AreEqual(EDbQueryStatus.Success, result.Status);
            Assert.AreEqual(users.Count, result.Users.Count);
            Assert.AreEqual(2, result.Messages.Count);
            mockUserService.Verify(x => x.GetUsersAsync(It.IsAny<string>()), Times.Once);
        }

        private static MessageService MockMessageService()
        {
            Mock<ChatDbContext> mockDb = new();
            Mock<IUserService> mockUserService = new();
            Mock<ILogger<MessageService>> mockLogger = new();
            Mock<IMessageRepository> mockMessageRepository = new();
            Mock<IUserRepository> mockUserRepository = new();

            return new(mockLogger.Object,
                mockUserService.Object,
                mockMessageRepository.Object,
                mockUserRepository.Object);
        }
    }
}
