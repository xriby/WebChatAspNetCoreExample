using System.Collections.Generic;
using Chat.Application.Identity;
using Chat.Application.ModelsDto;

namespace Chat.Application.Results
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
