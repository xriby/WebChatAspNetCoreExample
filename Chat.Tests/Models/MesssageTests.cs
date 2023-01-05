using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text.Json;
using Chat.Application.Models;
using Chat.Application.ModelsDto;

namespace Chat.Tests.Models
{
    [TestClass]
    public class MesssageTests
    {
        [TestMethod]
        public void TestOperatorMessageToDto()
        {
            int id = 1;
            Message message = new() { MessageId = id };
            MessageDto messageDto = (MessageDto)message;
            Assert.AreEqual(id, messageDto.MessageId);
        }
        
        [TestMethod]
        public void TestOperatorDtoToMessage()
        {
            int id = 2;
            MessageDto messageDto = new() { MessageId = id };
            Message message = (Message)messageDto;
            Assert.AreEqual(id, message.MessageId);
            Assert.IsNull(message.User);
        }
        
        [TestMethod]
        public void TestMessageSerialize()
        {
            int id = 1;
            MessageDto message = new() { MessageId = id, MessageType = EMessageType.Private };
            string json = JsonSerializer.Serialize(message);
            Debug.WriteLine(json);

            Assert.IsTrue(json.Contains("Private"));
        }
    }
}
