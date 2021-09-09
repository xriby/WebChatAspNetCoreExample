using Chat.Data.Models;
using Chat.Data.ModelsDto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
