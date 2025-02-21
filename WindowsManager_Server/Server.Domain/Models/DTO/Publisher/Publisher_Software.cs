namespace Server.Domain.Models.DTO.Publisher
{
    public class Publisher_Software
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Version { get; set; } = null!;

        public ICollection<Publisher_HostSoftware> Hosts { get; set; }
    }
} 