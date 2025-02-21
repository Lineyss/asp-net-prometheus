namespace Server.DataAccess.Entities;

public partial class HostSoftwareEntities
{
    public int Id { get; set; }

    public DateTime Added { get; set; }

    public int Hostid { get; set; }

    public int Softwareid { get; set; }

    public bool Isdeleted { get; set; } = false;

    public virtual HostEntities Host { get; set; } = null!;

    public virtual SoftwareEntities Software { get; set; } = null!;
}
