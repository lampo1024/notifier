using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notifier.Server.Entities.Configurations
{
    public class UtfUserConfiguration : IEntityTypeConfiguration<NtfUser>
    {
        public void Configure(EntityTypeBuilder<NtfUser> builder)
        {
            builder.ToTable("ntf_user");
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);
            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
