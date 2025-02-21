using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.DataAccess.Entities;

namespace Server.DataAccess.Configurations
{
    public class PublisherConfiguration : IEntityTypeConfiguration<PublisherEntities>
    {
        public void Configure(EntityTypeBuilder<PublisherEntities> builder)
        {
            builder.HasKey(e => e.Id).HasName("publisher_pkey");

            builder.ToTable("publisher");

            builder.HasIndex(e => e.Title, "publisher_title_key").IsUnique();

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        }
    }
}
