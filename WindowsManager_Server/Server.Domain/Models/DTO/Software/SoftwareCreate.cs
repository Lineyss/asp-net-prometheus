namespace Server.Domain.Models.DTO.Software
{
    public class SoftwareCreate
    {
        public string Name { get; set; } = null!;

        public string? Version { get; set; } = null!;

        public int? Publisherid { get; set; }
    }
}
