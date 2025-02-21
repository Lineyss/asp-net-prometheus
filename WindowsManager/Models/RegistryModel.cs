namespace WindowsManager.Models
{
    public class RegistryModel
    {
        public string DisplayName { get; }
        public string? DisplayVersion { get; }
        public string? Publisher { get; }
        public string? UninstallString { get; }
        public string? QuietUninstallString { get; }
        public string? DisplayIcon { get; }
        public string? InstallSource { get; }
        public string? InstallLocation { get; }
        public string? ModifyPath { get; }

        public RegistryModel(string DisplayName,
                             string? DisplayVersion,
                             string? Publisher,
                             string? UninstallString,
                             string? QuietUninstallString,
                             string? DisplayIcon,
                             string? InstallSource,
                             string? InstallLocation,
                             string? ModifyPath)
        {
            if (string.IsNullOrWhiteSpace(DisplayName))
                throw new Exception("DisplayName не может быть пустым");

            this.DisplayName = DisplayName;
            this.DisplayVersion = DisplayVersion;
            this.Publisher = Publisher;
            this.DisplayVersion = DisplayVersion;
            this.UninstallString = UninstallString;
            this.QuietUninstallString = QuietUninstallString;
            this.DisplayIcon = DisplayIcon;
            this.InstallSource = InstallSource;
            this.InstallLocation = InstallLocation;
            this.ModifyPath = ModifyPath;
        }        

        public override bool Equals(object obj)
        {
            if (obj is RegistryModel other)
                return DisplayName == other.DisplayName && DisplayVersion == other.DisplayVersion && Publisher == other.Publisher;

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(DisplayName, DisplayVersion, Publisher);
    }
}
