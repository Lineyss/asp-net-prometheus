namespace Server.Domain.Models.DTO.Host
{
    public class Host
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
        public ICollection<Host_HostSoftware> Softwares { get; set; }
    }
}
