using Server.Domain.Models.DTO.Host;
using Server.Domain.Models.DTO.Software;

namespace Server.Domain.Models.DTO.HostSoftware
{
    public class HostSoftwareCreated
    {
        public int Id { get; set; }
        public DateTime Added { get; set; }
        public int Hostid { get; set; }
        public int Softwareid { get; set; }
    }
}
