using Chat.Common;
using Chat.Data.Identity;
using Chat.Data.ModelsDto;
using System.Collections.Generic;

namespace Chat.Data.Common
{
    public class PrivateMessageInfoResult : DbQueryResultModel<int>
    {
        /// <summary>
        /// Сообщения.
        /// </summary>
        public List<MessageDto> Messages { get; set; }

        /// <summary>
        /// Пользователи.
        /// </summary>
        public List<ApplicationUser> Users { get; set; }

        /// <summary>
        /// Отправитель.
        /// </summary>
        public ApplicationUser FromUser { get; set; }

        /// <summary>
        /// Получатель.
        /// </summary>
        public ApplicationUser ToUser { get; set; }
    }
}
