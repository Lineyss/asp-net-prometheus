namespace Server.Domain.Models.DTO.Software
{
    public class Software_HostSoftware
    {
        public int Id { get; set; }
        public DateTime Added { get; set; }
        public Software_Host Host { get; set; } = null!;
    }
}