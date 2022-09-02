using Chat.Application.Identity;
using Chat.Application.Interfaces.Repositories;
using Chat.Infrastructure.Data;
using Chat.Infrastructure.Hosted;
using Chat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string efConnectionString = configuration.GetConnectionString("PostresConnection");

            services.AddDbContext<ChatDbContext>(options =>
                options.UseNpgsql(efConnectionString,
                    x => x.MigrationsAssembly("Chat.Infrastructure")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ChatDbContext>();

            services.AddHostedService<MigrateService>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            return services;
        }
    }
}
