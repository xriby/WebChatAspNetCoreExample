using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Chat.Infrastructure.Models;

namespace Chat.Infrastructure.ModelsDto
{
    /// <summary>
    /// Модель сообщения передачи данных.
    /// </summary>
    public class MessageDto
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Display(Name = "Идентификатор")]
        public int MessageId { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        [Required(ErrorMessage = "Введите текст")]
        [MaxLength(512, ErrorMessage = "Максимальная длина 512 символов")]
        [MinLength(2, ErrorMessage = "Минимальная длина 2 символа")]
        [Display(Name = "Текст сообщения")]
        public string Text { get; set; }

        /// <summary>
        /// Тип сообщения.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EMessageType MessageType { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Статус просмотра сообщения.
        /// </summary>
        public bool Viewed { get; set; }

        /// <summary>
        /// Получатель сообщения.
        /// </summary>
        public string RecipientId { get; set; }

        /// <summary>
        /// Имя отправителя сообщения.
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// Идентификатор отправителя сообщения.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Перегружаем операцию преобразования
        /// </summary>
        public static explicit operator MessageDto(Message message)
        {
            return new MessageDto
            {
                MessageId = message.MessageId,
                Text = message.Text,
                CreateDate = message.CreateDate,
                MessageType = message.MessageType,
                RecipientId = message.RecipientId,
                Viewed = message.Viewed,
                UserId = message.User?.Id,
                UserName = message.User?.UserName
            };
        }
    }
}
