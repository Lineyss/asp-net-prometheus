using Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.DataAccess.Configurations
{
    public class HostSoftwareConfiguration : IEntityTypeConfiguration<HostSoftwareEntities>
    {
        public void Configure(EntityTypeBuilder<HostSoftwareEntities> builder)
        {
            builder.HasKey(e => e.Id).HasName("host_software_pkey");

            builder.ToTable("host_software");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Added)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("added");
            builder.Property(e => e.Hostid).HasColumnName("hostid");
            builder.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            builder.Property(e => e.Softwareid).HasColumnName("softwareid");

            builder.HasOne(d => d.Host).WithMany(p => p.HostSoftwares)
                .HasForeignKey(d => d.Hostid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("host_software_hostid_fkey");

            builder.HasOne(d => d.Software)
                .WithMany(p => p.HostSoftwares)
                .HasForeignKey(d => d.Softwareid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("host_software_softwareid_fkey");
        }
    }
}
