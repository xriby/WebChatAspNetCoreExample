using Chat.Application.Results;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Web.ViewModels
{
    public class MessageInfoVm
    {
        /// <summary>
        /// Сообщения.
        /// </summary>
        public List<MessageVm> Messages { get; set; }

        /// <summary>
        /// Пользователи.
        /// </summary>
        public List<UserVm> Users { get; set; }

        public static explicit operator MessageInfoVm(MessageInfoResult messageInfo)
        {
            return new MessageInfoVm
            {
                Messages = messageInfo.Messages.Select(m => (MessageVm)m).ToList(),
                Users = messageInfo.Users.Select(u => (UserVm)u).ToList(),
            };
        }
    }
}
