namespace Server.Domain.Models.DTO.Host
{
    public class Host_HostSoftware
    {
        public int Id { get; set; }
        public DateTime Added { get; set; }
        public Host_Software Software { get; set; }
    }
}