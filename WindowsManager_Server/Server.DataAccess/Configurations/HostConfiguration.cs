using Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.DataAccess.Configurations
{
    public class HostConfiguration : IEntityTypeConfiguration<HostEntities>
    {
        public void Configure(EntityTypeBuilder<HostEntities> builder)
        {
            builder.HasKey(e => e.Id).HasName("host_pkey");

            builder.ToTable("host");

            builder.HasIndex(e => e.Hostname, "host_hostname_key").IsUnique();

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Hostname)
                .HasMaxLength(255)
                .HasColumnName("hostname");
        }
    }
}
