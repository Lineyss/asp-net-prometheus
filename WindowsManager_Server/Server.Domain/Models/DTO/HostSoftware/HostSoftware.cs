using Server.Domain.Models.DTO.Host;

namespace Server.Domain.Models.DTO.HostSoftware
{
    public class HostSoftware
    {
        public int Id { get; set; }
        public DateTime Added { get; set; }
        public HostCreated Host { get; set; }
        public HostSoftware_Software Software { get; set; }
    }
}
