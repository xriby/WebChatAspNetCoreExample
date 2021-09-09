using Chat.Data.Identity;
using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.ModelsConfiguration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(p => p.MessageId);

            builder.Property(p => p.Text)
                .HasMaxLength(512)
                .IsRequired();
            
            builder.Property(p => p.RecipientId)
                .HasMaxLength(256)
                .IsRequired(false);

            builder.HasOne<ApplicationUser>(x => x.User)
                .WithMany(x => x.Messages);
        }
    }
}
