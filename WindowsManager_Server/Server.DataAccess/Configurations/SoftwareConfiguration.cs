using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.DataAccess.Entities;

namespace Server.DataAccess.Configurations
{
    public class SoftwareConfiguration : IEntityTypeConfiguration<SoftwareEntities>
    {
        public void Configure(EntityTypeBuilder<SoftwareEntities> builder)
        {
            builder.HasKey(e => e.Id).HasName("software_pkey");

            builder.ToTable("software");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            builder.Property(e => e.Publisherid).HasColumnName("publisherid");
            builder.Property(e => e.Version)
                .HasMaxLength(255)
                .HasColumnName("version");

            builder.HasOne(d => d.Publisher)
                .WithMany(p => p.Softwares)
                .HasForeignKey(d => d.Publisherid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("software_publisherid_fkey");
        }
    }
}
