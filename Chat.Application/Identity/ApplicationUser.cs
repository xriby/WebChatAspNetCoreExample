using System.Collections.Generic;
using Chat.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Chat.Application.Identity
{
    /// <summary>
    /// Модель пользователя.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Адрес.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Сообщения.
        /// </summary>
        public virtual ICollection<Message> Messages { get; set; }
        
    }
}
