namespace Server.Domain.Models.DTO.Publisher
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Publisher_Software> Softwares { get; set; }
    }
}
