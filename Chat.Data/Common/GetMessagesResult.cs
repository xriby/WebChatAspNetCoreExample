using Chat.Common;
using Chat.Data.Models;
using System.Collections.Generic;

namespace Chat.Data.Common
{
    public class GetMessagesResult : DbQueryResultModel<int>
    {
        /// <summary>
        /// Сообщения.
        /// </summary>
        public List<Message> Messages { get; set; }
    }
}
