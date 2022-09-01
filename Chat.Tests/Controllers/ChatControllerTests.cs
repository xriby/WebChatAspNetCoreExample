using Chat.Common;
using Chat.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Chat.Application.Interfaces;
using Chat.Infrastructure.Common;
using Chat.Infrastructure.Identity;
using Chat.Infrastructure.Models;
using Chat.Infrastructure.ModelsDto;

namespace Chat.Tests.Controllers
{
    [TestClass]
    public class ChatControllerTests
    {
        [TestMethod]
        public async Task Index()
        {
            var mockUserService = new Mock<IUserService>();
            var mockMessageService = new Mock<IMessageService>();
            var mockLogger = new Mock<ILogger<ChatController>>();
            var messageInfoResult = new MessageInfoResult
            {
                Status = EDbQueryStatus.Success,
                Messages = new List<MessageDto>(),
                Users = new List<ApplicationUser>()
            };
            mockMessageService.Setup(x => x.GetMessageInfoAsync(It.IsAny<string>())).ReturnsAsync(messageInfoResult);
            var controller = new ChatController(mockLogger.Object, mockMessageService.Object, mockUserService.Object);

            ViewResult result = await controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("index", result.ViewName.ToLower());
            mockMessageService.Verify(x => x.GetMessageInfoAsync(It.IsAny<string>()), Times.Once);
        }
        
        [TestMethod]
        public async Task Add()
        {
            var mockUserService = new Mock<IUserService>();
            var mockMessageService = new Mock<IMessageService>();
            var mockLogger = new Mock<ILogger<ChatController>>();
            var message = new MessageDto 
            { 
                MessageId = 1, 
                Text = "Message1", 
                CreateDate = DateTime.UtcNow, 
                MessageType = EMessageType.Public 
            };
            var addMessageResult = new AddMessageResult
            {
                Status = EDbQueryStatus.Success,
                Data = message
            };
            mockMessageService.Setup(x => x.AddMessageAsync(message, It.IsAny<string>())).ReturnsAsync(addMessageResult);
            var controller = new ChatController(mockLogger.Object, mockMessageService.Object, mockUserService.Object);

            ContentResult result = await controller.Add(message) as ContentResult;

            Assert.IsNotNull(result);
            JsonDocument document = JsonDocument.Parse(result.Content);
            JsonElement dataElement = document.RootElement.GetProperty("Data");
            MessageDto addedMessage = JsonSerializer.Deserialize<MessageDto>(dataElement.GetRawText());
            Assert.AreEqual(message.MessageId, addedMessage.MessageId);
            Assert.AreEqual(message.Text, addedMessage.Text);
            mockMessageService.Verify(x => x.AddMessageAsync(It.IsAny<MessageDto>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task PrivateFailure()
        {
            var mockUserService = new Mock<IUserService>();
            var mockMessageService = new Mock<IMessageService>();
            var mockLogger = new Mock<ILogger<ChatController>>();
            string user = "user1";
            var privateMessageInfoResult = new PrivateMessageInfoResult
            {
                Status = EDbQueryStatus.Failure,
                Messages = new List<MessageDto>(),
                Users = new List<ApplicationUser>()
            };
            mockMessageService.Setup(x => x.GetPrivateMessageInfoAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(privateMessageInfoResult);
            var controller = new ChatController(mockLogger.Object, mockMessageService.Object, mockUserService.Object);

            ViewResult result = await controller.Private(user) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("error", result.ViewName.ToLower());
            Assert.IsTrue(result.Model is ErrorViewModel);
            mockMessageService.Verify(x => x.GetPrivateMessageInfoAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        
        [TestMethod]
        public async Task PrivateSuccess()
        {
            var mockUserService = new Mock<IUserService>();
            var mockMessageService = new Mock<IMessageService>();
            var mockLogger = new Mock<ILogger<ChatController>>();
            string user = "user1";
            var privateMessageInfoResult = new PrivateMessageInfoResult
            {
                Status = EDbQueryStatus.Success,
                Messages = new List<MessageDto>(),
                Users = new List<ApplicationUser>()
            };
            mockMessageService.Setup(x => x.GetPrivateMessageInfoAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(privateMessageInfoResult);
            var controller = new ChatController(mockLogger.Object, mockMessageService.Object, mockUserService.Object);

            ViewResult result = await controller.Private(user) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("private", result.ViewName.ToLower());
            Assert.IsTrue(result.Model is PrivateMessageInfoResult);
            mockMessageService.Verify(x => x.GetPrivateMessageInfoAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
