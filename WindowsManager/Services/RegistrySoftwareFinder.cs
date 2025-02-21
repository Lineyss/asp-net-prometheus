using Microsoft.Win32;
using WindowsManager.Models;
using WindowsManager.Abstracts;

namespace WindowsManager.Services
{
    public class RegistrySoftwareFinder : ISoftwareFinder
    {
        protected readonly RegistryKey[] dirs =
        [
            Registry.LocalMachine,
            Registry.CurrentUser,
            Registry.Users
        ];

        protected string[] keys = new[]
        {
            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
            "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
        };

        public async Task<HashSet<RegistryModel>> Find(CancellationToken token)
        {
            HashSet<RegistryModel> softwares = new HashSet<RegistryModel>();

            foreach (var dir in dirs)
            {
                foreach(var key in keys)
                {
                    HashSet<RegistryModel> get = await ReadProgramsFromRegistry(dir, key, token);
                    softwares = softwares.Concat(get).ToHashSet();
                    if (token.IsCancellationRequested)
                        return softwares;
                }
            }

            return softwares;
        }

        protected virtual async Task<HashSet<RegistryModel>> ReadProgramsFromRegistry(RegistryKey baseKey, string subKeyPath, CancellationToken token)
        {
            HashSet<RegistryModel> programs = new HashSet<RegistryModel>();

            using (var subKey = baseKey.OpenSubKey(subKeyPath))
            {
                if (subKey != null)
                {
                    var subKeyNames = subKey.GetSubKeyNames();
                    foreach(var subKeyName in subKeyNames)
                    {
                        if (token.IsCancellationRequested) 
                            break;

                        RegistryModel? software = GetSoftwareFromKey(subKey, subKeyName);

                        if(software != null)
                            programs.Add(software);
                    }
                }
            }

            return programs;
        }

        protected virtual RegistryModel? GetSoftwareFromKey(RegistryKey subKey, string subKeyName)
        {
            using (var programKey = subKey.OpenSubKey(subKeyName))
            {
                string? Publisher = programKey?.GetValue("Publisher") as string;
                string? ModifyPath = programKey?.GetValue("ModifyPath") as string;
                string? DisplayIcon = programKey?.GetValue("DisplayIcon") as string;
                string? DisplayName = programKey?.GetValue("DisplayName") as string;
                string? InstallSource = programKey?.GetValue("InstallSource") as string;
                string? DisplayVersion = programKey?.GetValue("DisplayVersion") as string;
                string? InstallLocation = programKey?.GetValue("InstallLocation") as string;
                string? UninstallString = programKey?.GetValue("UninstallString") as string;
                string? QuietUninstallString = programKey?.GetValue("QuietUninstallString") as string;


                if (!string.IsNullOrEmpty(DisplayName))
                    return new RegistryModel(DisplayName, 
                                             DisplayVersion, 
                                             Publisher, 
                                             UninstallString, 
                                             QuietUninstallString, 
                                             DisplayIcon, 
                                             InstallSource, 
                                             InstallLocation, 
                                             ModifyPath);
            }

            return null;
        }
    }
}
