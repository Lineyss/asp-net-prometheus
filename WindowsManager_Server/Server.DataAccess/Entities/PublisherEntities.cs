namespace Server.DataAccess.Entities;

public partial class PublisherEntities
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<SoftwareEntities> Softwares { get; set; } = new List<SoftwareEntities>();
}
