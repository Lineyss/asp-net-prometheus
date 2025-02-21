namespace Server.Domain.Models.DTO.Publisher
{
    public class Publisher_HostSoftware
    {
        public int Id { get; set; }

        public DateTime Added { get; set; }

        public Publisher_Host Host { get; set; } = null!;
    }
}