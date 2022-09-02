using System.Collections.Generic;
using Chat.Application.Identity;
using Chat.Application.ModelsDto;

namespace Chat.Application.Results
{
    public class MessageInfoResult : DbQueryResultModel<int>
    {
        /// <summary>
        /// Сообщения.
        /// </summary>
        public List<MessageDto> Messages { get; set; }

        /// <summary>
        /// Пользователи.
        /// </summary>
        public List<ApplicationUser> Users { get; set; }
    }
}
