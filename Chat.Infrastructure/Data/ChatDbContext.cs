using Chat.Application.Identity;
using Chat.Application.Models;
using Chat.Infrastructure.ModelsConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Data
{
    public class ChatDbContext : IdentityDbContext<ApplicationUser>
    {
        public ChatDbContext()
        {
        }

        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
        }
    }
}
