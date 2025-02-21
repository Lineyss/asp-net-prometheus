namespace Server.Domain.Models
{
    public class Error
    {
        public Error(string Title, int Status)
        {
            this.Title = Title;
            this.Status = Status;
        }

        public Error(string Title, int Status, IEnumerable<Violation> Violations)
        {
            this.Title = Title;
            this.Status = Status;
            this.Violations = Violations;
        }

        public string Title { get; }
        public int Status { get; }
        public IEnumerable<Violation>? Violations { get; }
    }
}
