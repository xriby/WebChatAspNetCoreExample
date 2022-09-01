using System.Collections.Generic;
using Chat.Common;
using Chat.Infrastructure.Identity;

namespace Chat.Infrastructure.Common
{
    public class GetUsersResult : DbQueryResultModel<List<ApplicationUser>>
    {
    }
}
