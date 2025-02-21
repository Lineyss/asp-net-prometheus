namespace Server.Domain.Models.DTO.Software
{
    public class SoftwareCreated
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Version { get; set; }

        public int? Publisherid { get; set; }
    }
}
