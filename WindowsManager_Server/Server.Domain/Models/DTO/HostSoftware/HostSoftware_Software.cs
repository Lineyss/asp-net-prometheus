using Server.Domain.Models.DTO.Publisher;

namespace Server.Domain.Models.DTO.HostSoftware
{
    public class HostSoftware_Software
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Version { get; set; }
        public PublisherCreated? Publisher { get; set; }
    }
}
