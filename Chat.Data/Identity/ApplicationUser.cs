using Chat.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Chat.Data.Identity
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
