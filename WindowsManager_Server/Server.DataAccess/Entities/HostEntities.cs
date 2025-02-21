namespace Server.DataAccess.Entities;

public partial class HostEntities
{
    public int Id { get; set; }

    public string Hostname { get; set; } = null!;

    public virtual ICollection<HostSoftwareEntities> HostSoftwares { get; set; } = new List<HostSoftwareEntities>();
}
