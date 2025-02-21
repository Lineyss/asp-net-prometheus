namespace Server.Domain.Models
{
    public class Violation
    {
        public Violation(string Field, string Message)
        {
            this.Field = Field;
            this.Message = Message;
        }

        public string Field { get; }
        public string Message { get; }
    }
}
