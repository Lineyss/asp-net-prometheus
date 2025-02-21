namespace Server.Domain.Models.DTO.Software
{
    public class Software
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Version { get; set; } = null!;

        public ICollection<Software_HostSoftware> Hosts { get; set; } = new List<Software_HostSoftware>();

        public Software_Publisher? Publisher { get; set; }
    }
}
