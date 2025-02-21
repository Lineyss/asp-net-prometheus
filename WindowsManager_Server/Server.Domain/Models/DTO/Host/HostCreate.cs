using System.Reflection.Metadata;

namespace Server.Domain.Models.DTO.Host
{
    public class HostCreate
    {
        public string Hostname { get; set; }
        public List<Host_AddSoftwares> Softwares { get; set; }
    }
}
