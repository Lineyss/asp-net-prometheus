namespace Server.DataAccess.Entities;

public partial class SoftwareEntities
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Version { get; set; } 

    public int? Publisherid { get; set; }

    public ICollection<HostSoftwareEntities> HostSoftwares { get; set; } = new List<HostSoftwareEntities>();

    public PublisherEntities? Publisher { get; set; }
}

