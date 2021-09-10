using Chat.Data.Identity;
using Chat.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chat.Data.ModelsDto
{
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
        /// Получатель сообщения.
        /// </summary>
        public string RecipientUserName { get; set; }

        /// <summary>
        /// Отправитель сообщения.
        /// </summary>
        public ApplicationUser User { get; set; }

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
                User = message.User
            };
        }
    }
}
