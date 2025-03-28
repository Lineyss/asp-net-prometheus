using Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Server.DataAccess.Configurations;

namespace Server.DataAccess;

public partial class DataBaseContext : DbContext
{
    public virtual DbSet<HostEntities> Hosts { get; set; }

    public virtual DbSet<HostSoftwareEntities> HostSoftwares { get; set; }

    public virtual DbSet<PublisherEntities> Publishers { get; set; }

    public virtual DbSet<SoftwareEntities> Softwares { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=postgres_server2:5432;Database=windows_hosts;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HostConfiguration());
        modelBuilder.ApplyConfiguration(new SoftwareConfiguration());
        modelBuilder.ApplyConfiguration(new PublisherConfiguration());
        modelBuilder.ApplyConfiguration(new HostSoftwareConfiguration());
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
