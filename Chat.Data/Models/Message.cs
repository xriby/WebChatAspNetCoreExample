using Chat.Data.Identity;
using Chat.Data.ModelsDto;
using System;

namespace Chat.Data.Models
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

        /// <summary>
        /// Перегружаем операцию преобразования
        /// </summary>
        public static explicit operator Message(MessageDto messageDto)
        {
            return new Message
            {
                MessageId = messageDto.MessageId,
                Text = messageDto.Text,
                CreateDate = messageDto.CreateDate,
                MessageType = messageDto.MessageType,
                RecipientId = messageDto.RecipientId,
                Viewed = messageDto.Viewed,
                User = messageDto.User
            };
        }
    }
}
