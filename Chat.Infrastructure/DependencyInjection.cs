using Chat.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string efConnectionString = configuration.GetConnectionString("PostresConnection");

            services.AddDbContext<ChatDbContext>(options =>
                options.UseNpgsql(efConnectionString,
                    x => x.MigrationsAssembly("Chat.Infrastructure")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ChatDbContext>();

            return services;
        }
    }
}
