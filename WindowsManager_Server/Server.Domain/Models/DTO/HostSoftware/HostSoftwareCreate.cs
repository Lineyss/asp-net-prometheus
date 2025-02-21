using Server.Domain.Models.DTO.Host;
using Server.Domain.Models.DTO.Software;

namespace Server.Domain.Models.DTO.HostSoftware
{
    public class HostSoftwareCreate
    {
        public int Hostid { get; set; }
        public int Softwareid { get; set; }
    }
}
