namespace WindowsManager.Models
{
    public class HttpSoftware
    {
        public HttpSoftware(string Name, string? Version, string? Publisher)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new Exception("Name не может быть пустым");

            this.Name = Name;
            this.Publisher = Publisher;
            this.Version = Version;
        }

        public string Name { get; set; }
        public string? Version { get; set; }
        public string? Publisher { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is HttpSoftware other) 
                return Name == other.Name && Version == other.Version && Publisher == other.Publisher;

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Name, Version, Publisher);
    }
}
