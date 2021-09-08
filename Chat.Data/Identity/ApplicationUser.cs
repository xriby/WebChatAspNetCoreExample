using Microsoft.AspNetCore.Identity;

namespace Chat.Data.Identity
{
    /// <summary>
    /// Модель пользователя.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
    }
}
