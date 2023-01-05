using Chat.Application.Identity;

namespace Chat.Web.ViewModels
{
    public class UserVm
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public static explicit operator UserVm(ApplicationUser user)
        {
            return new UserVm
            {
                Id = user?.Id,
                UserName = user?.UserName,
            };
        }
    }
}