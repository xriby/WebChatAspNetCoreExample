using Chat.Common;
using Chat.Data.ModelsDto;
using System.Collections.Generic;

namespace Chat.Data.Common
{
    public class GetMessageResult : DbQueryResultModel<int>
    {
        /// <summary>
        /// Сообщения.
        /// </summary>
        public List<MessageDto> Messages { get; set; }
    }
}
