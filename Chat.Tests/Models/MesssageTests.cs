using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text.Json;
using Chat.Infrastructure.Models;
using Chat.Infrastructure.ModelsDto;

namespace Chat.Tests.Models
{
    [TestClass]
    public class MesssageTests
    {
        [TestMethod]
        public void TestOperatorMessageToDto()
        {
            int id = 1;
            var message = new Message { MessageId = id };
            MessageDto messageDto = (MessageDto)message;
            Assert.AreEqual(id, messageDto.MessageId);
        }
        
        [TestMethod]
        public void TestOperatorDtoToMessage()
        {
            int id = 2;
            var messageDto = new MessageDto { MessageId = id };
            Message message = (Message)messageDto;
            Assert.AreEqual(id, message.MessageId);
            Assert.IsNull(message.User);
        }
        
        [TestMethod]
        public void TestMessageSerialize()
        {
            int id = 1;
            var message = new MessageDto { MessageId = id, MessageType = EMessageType.Private };
            string json = JsonSerializer.Serialize(message);
            Debug.WriteLine(json);

            Assert.IsTrue(json.Contains("Private"));
        }
    }
}
