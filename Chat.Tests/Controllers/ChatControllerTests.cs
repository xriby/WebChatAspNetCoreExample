using Chat.Application.Identity;
using Chat.Application.Interfaces;
using Chat.Application.Models;
using Chat.Application.ModelsDto;
using Chat.Application.Results;
using Chat.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Chat.Web.ViewModels;

namespace Chat.Tests.Controllers
{
    [TestClass]
    public class ChatControllerTests
    {
        [TestMethod]
        public async Task Index()
        {
            Mock<IUserService> mockUserService = new();
            Mock<IMessageService> mockMessageService = new();
            Mock<ILogger<ChatController>> mockLogger = new();
            MessageInfoResult messageInfoResult = new()
            {
                Status = EDbQueryStatus.Success,
                Messages = new(),
                Users = new()
            };
            mockMessageService.Setup(x => x.GetMessageInfoAsync(It.IsAny<string>())).ReturnsAsync(messageInfoResult);
            ChatController controller = new(mockMessageService.Object, mockUserService.Object);

            ViewResult result = await controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("index", result.ViewName.ToLower());
            mockMessageService.Verify(x => x.GetMessageInfoAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Add()
        {
            Mock<IUserService> mockUserService = new();
            Mock<IMessageService> mockMessageService = new();
            Mock<ILogger<ChatController>> mockLogger = new();
            MessageDto message = new()
            {
                MessageId = 1,
                Text = "Message1",
                CreateDate = DateTime.UtcNow,
                MessageType = EMessageType.Public
            };
            AddMessageResult addMessageResult = new()
            {
                Status = EDbQueryStatus.Success,
                Data = message
            };
            mockMessageService.Setup(x => x.AddMessageAsync(message, It.IsAny<string>())).ReturnsAsync(addMessageResult);
            ChatController controller = new(mockMessageService.Object, mockUserService.Object);

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
            Mock<IUserService> mockUserService = new();
            Mock<IMessageService> mockMessageService = new();
            Mock<ILogger<ChatController>> mockLogger = new();
            string user = "user1";
            PrivateMessageInfoResult privateMessageInfoResult = new()
            {
                Status = EDbQueryStatus.Failure,
                Messages = new(),
                Users = new()
            };
            mockMessageService.Setup(x => x.GetPrivateMessageInfoAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(privateMessageInfoResult);
            ChatController controller = new(mockMessageService.Object, mockUserService.Object);

            ViewResult result = await controller.Private(user) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("error", result.ViewName.ToLower());
            Assert.IsTrue(result.Model is ErrorViewModel);
            mockMessageService.Verify(x => x.GetPrivateMessageInfoAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task PrivateSuccess()
        {
            Mock<IUserService> mockUserService = new();
            Mock<IMessageService> mockMessageService = new();
            Mock<ILogger<ChatController>> mockLogger = new();
            string user = "user1";
            PrivateMessageInfoResult privateMessageInfoResult = new()
            {
                Status = EDbQueryStatus.Success,
                Messages = new(),
                Users = new()
            };
            mockMessageService.Setup(x => x.GetPrivateMessageInfoAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(privateMessageInfoResult);
            ChatController controller = new(mockMessageService.Object, mockUserService.Object);

            ViewResult result = await controller.Private(user) as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("private", result.ViewName.ToLower());
            Assert.IsTrue(result.Model is PrivateMessageInfoResult);
            mockMessageService.Verify(x => x.GetPrivateMessageInfoAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
