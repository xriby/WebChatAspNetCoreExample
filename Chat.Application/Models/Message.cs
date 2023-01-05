using Chat.Application.Identity;
using System;

namespace Chat.Application.Models
{
    /// <summary>
    /// Модель сообщения.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Тип сообщения.
        /// </summary>
        public EMessageType MessageType { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Статус просмотра сообщения..
        /// </summary>
        public bool Viewed { get; set; }

        /// <summary>
        /// Получатель сообщения.
        /// </summary>
        public string RecipientId { get; set; }

        /// <summary>
        /// Пользователь отправитель.
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }
}
