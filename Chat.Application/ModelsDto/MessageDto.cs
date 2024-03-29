﻿using Chat.Application.Models;
using System;
using System.Text.Json.Serialization;

namespace Chat.Application.ModelsDto
{
    /// <summary>
    /// Модель сообщения передачи данных.
    /// </summary>
    public class MessageDto
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
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
            };
        }
    }
}
