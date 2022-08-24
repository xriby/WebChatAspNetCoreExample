using Chat.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string efConnectionString = configuration.GetConnectionString("PostresConnection");

            services.AddDbContext<ChatDbContext>(options =>
                options.UseNpgsql(efConnectionString,
                    x => x.MigrationsAssembly("Chat.Data")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ChatDbContext>();

            return services;
        }
    }
}
