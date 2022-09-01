using System.Collections.Generic;
using Chat.Common;
using Chat.Infrastructure.Identity;
using Chat.Infrastructure.ModelsDto;

namespace Chat.Infrastructure.Common
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
