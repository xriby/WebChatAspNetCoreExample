﻿using Chat.Common;
using Chat.Data;
using Chat.Data.Common;
using Chat.Data.Identity;
using Chat.Data.Models;
using Chat.Data.ModelsDto;
using Chat.Services;
using Chat.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Tests.Services
{
    [TestClass]
    public class MessageServiceTests
    {
        [TestMethod]
        public async Task AddEmptyMessageAsync()
        {
            //Arrange
            MessageService messageService = MockMessageService();

            var message = new MessageDto();
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

            var message = new MessageDto { Text = new string('a', moreThanMaxText) };
            string user = "user";

            AddMessageResult result = await messageService.AddMessageAsync(message, user);

            Assert.AreEqual(EDbQueryStatus.Failure, result.Status);
            Assert.IsTrue(result.ErrorMessage.Contains("Ошибка"));
        }
        
        [TestMethod]
        public async Task EmptyUserAddMessageAsync()
        {
            MessageService messageService = MockMessageService();

            var message = new MessageDto { Text = "Message" };

            AddMessageResult result = await messageService.AddMessageAsync(message, null);

            Assert.AreEqual(EDbQueryStatus.Failure, result.Status);
            Assert.AreEqual("Ошибка. Не задан пользователь.", result.ErrorMessage);
        }
        
        [TestMethod]
        public async Task NotFoundUserAddMessageAsync()
        {
            Guid giud1 = Guid.NewGuid();
            Guid giud2 = Guid.NewGuid();
            var user1 = new ApplicationUser { Id = giud1.ToString(), UserName = "user1" };
            var user2 = new ApplicationUser { Id = giud2.ToString(), UserName = "user2" };
            var users = new List<ApplicationUser> { user1, user2 };
            var messages = new List<Message>
            {
                new Message
                { 
                    MessageId = 1, 
                    Text = "Message1", 
                    MessageType = EMessageType.Public, 
                    CreateDate = DateTime.UtcNow,
                    User = user1
                },
                new Message
                { 
                    MessageId = 2, 
                    Text = "Message2", 
                    MessageType = EMessageType.Public, 
                    CreateDate = DateTime.UtcNow,
                    User = user2
                }
            };
            
            var mockDb = new Mock<ChatDbContext>();
            var mockUserService = new Mock<IUserService>();
            var mockLogger = new Mock<ILogger<MessageService>>();
            
            var mockUsers = users.AsQueryable().BuildMockDbSet();
            var mockMessages = messages.AsQueryable().BuildMockDbSet();
            
            mockDb.Setup(x => x.Users).Returns(mockUsers.Object);
            mockDb.Object.Messages = mockMessages.Object;

            mockMessages.Setup(_ => _.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
                .Callback((Message model, CancellationToken token) => { messages.Add(model); })
                .ReturnsAsync((Message model, CancellationToken token) => null);

            var messageService = new MessageService(mockLogger.Object,
                mockUserService.Object,
                mockDb.Object);
            
            var message = new MessageDto { Text = "Message" };
            string user = "user";
           
            AddMessageResult result = await messageService.AddMessageAsync(message, user);

            Assert.AreEqual(EDbQueryStatus.Failure, result.Status);
            Assert.AreEqual("Ошибка. Пользователь не найден.", result.ErrorMessage);
            mockMessages.Verify(x => x.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Never);
            mockDb.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
        
        [TestMethod]
        public async Task AddMessageAsync()
        {
            Guid giud1 = Guid.NewGuid();
            Guid giud2 = Guid.NewGuid();
            var user1 = new ApplicationUser { Id = giud1.ToString(), UserName = "user1" };
            var user2 = new ApplicationUser { Id = giud2.ToString(), UserName = "user2" };
            var users = new List<ApplicationUser> { user1, user2 };
            var messages = new List<Message>
            {
                new Message
                { 
                    MessageId = 1, 
                    Text = "Message1", 
                    MessageType = EMessageType.Public, 
                    CreateDate = DateTime.UtcNow,
                    User = user1
                },
                new Message
                { 
                    MessageId = 2, 
                    Text = "Message2", 
                    MessageType = EMessageType.Public, 
                    CreateDate = DateTime.UtcNow,
                    User = user2
                }
            };
            
            var mockDb = new Mock<ChatDbContext>();
            var mockUserService = new Mock<IUserService>();
            var mockLogger = new Mock<ILogger<MessageService>>();
            
            var mockUsers = users.AsQueryable().BuildMockDbSet();
            var mockMessages = messages.AsQueryable().BuildMockDbSet();
            
            mockDb.Setup(x => x.Users).Returns(mockUsers.Object);
            mockDb.Object.Messages = mockMessages.Object;

            mockMessages.Setup(_ => _.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
                .Callback((Message model, CancellationToken token) => { messages.Add(model); })
                .ReturnsAsync((Message model, CancellationToken token) => null);

            var messageService = new MessageService(mockLogger.Object,
                mockUserService.Object,
                mockDb.Object);
            
            var message = new MessageDto { Text = "Message" };
            string user = "user1";
           
            AddMessageResult result = await messageService.AddMessageAsync(message, user);

            Assert.AreEqual(EDbQueryStatus.Success, result.Status);
            mockMessages.Verify(x => x.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Once);
            mockDb.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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
            MessageService messageService = MockMessageService();

            string user = "user1";

            MessageInfoResult result = await messageService.GetMessageInfoAsync(user);

            Assert.AreEqual(EDbQueryStatus.Failure, result.Status);
            
        }

        private static MessageService MockMessageService()
        {
            var mockDb = new Mock<ChatDbContext>();
            var mockUserService = new Mock<IUserService>();
            var mockLogger = new Mock<ILogger<MessageService>>();

            return new MessageService(mockLogger.Object,
                mockUserService.Object,
                mockDb.Object);
        }
    }
}