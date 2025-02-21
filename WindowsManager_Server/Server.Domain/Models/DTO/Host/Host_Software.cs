namespace Server.Domain.Models.DTO.Host
{
    public class Host_Software
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public Host_Publisher Publisher { get; set; }
    }
}