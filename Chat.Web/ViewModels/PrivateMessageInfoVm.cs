using Chat.Application.Results;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Web.ViewModels
{
    public class PrivateMessageInfoVm
    {
        /// <summary>
        /// Сообщения.
        /// </summary>
        public List<MessageVm> Messages { get; set; }

        /// <summary>
        /// Пользователи.
        /// </summary>
        public List<UserVm> Users { get; set; }

        /// <summary>
        /// Отправитель.
        /// </summary>
        public UserVm FromUser { get; set; }

        /// <summary>
        /// Получатель.
        /// </summary>
        public UserVm ToUser { get; set; }

        public static explicit operator PrivateMessageInfoVm(PrivateMessageInfoResult info)
        {
            return new PrivateMessageInfoVm
            {
                Messages = info.Messages.Select(m => (MessageVm)m).ToList(),
                Users = info.Users.Select(u => (UserVm)u).ToList(),
                FromUser = (UserVm)info.FromUser,
                ToUser = (UserVm)info.ToUser,
            };
        }
    }
}
