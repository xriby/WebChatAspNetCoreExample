using System.Collections.Generic;
using Chat.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Chat.Infrastructure.Identity
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
