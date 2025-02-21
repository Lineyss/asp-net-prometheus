using Microsoft.Win32;
using WindowsManager.Models;
using WindowsManager.Abstracts;

namespace WindowsManager.Services
{
    public class DeleteSoftwareFolder(ILogger<DeleteSoftwareFolder> logger) : RegistrySoftwareFinder, ISoftwareDelete
    {
        private RegistryModel findSoftware;
        private HttpSoftware software_for_delete;
        private readonly ILogger<DeleteSoftwareFolder> logger = logger;
        private CancellationTokenSource tokenSource;
        public async Task Delete(HttpSoftware software)
        {
            tokenSource = new CancellationTokenSource();
            software_for_delete = software;
            findSoftware = null;

            await Find(tokenSource.Token);

            if (findSoftware == null)
                return;

            if(findSoftware.UninstallString?.Contains("steam.exe") == true ||
               findSoftware.QuietUninstallString?.Contains("steam.exe") == true)
            {
                CleanUpFilesAndDirectories(findSoftware.DisplayIcon,
                       findSoftware.InstallLocation,
                       findSoftware.InstallSource,
                       findSoftware.ModifyPath);
            }
            else
            {
                CleanUpFilesAndDirectories(findSoftware.DisplayIcon,
                       findSoftware.InstallLocation,
                       findSoftware.InstallSource,
                       findSoftware.QuietUninstallString,
                       findSoftware.ModifyPath,
                       findSoftware.UninstallString);
            }
        }
        private void CleanUpFilesAndDirectories(params string[] strings_for_delete)
        {
            try
            {
                string path = string.Empty;
                int countDeleted = 0;
                foreach(string string_for_delete in strings_for_delete.Where(element => !string.IsNullOrWhiteSpace(element)))
                {
                    if (!IsSystemPathExist(string_for_delete)) 
                        path = ExtractPath(string_for_delete);
                    else 
                        path = string_for_delete;

                    if (!string.IsNullOrWhiteSpace(path) && IsSystemPathExist(path))
                    {
                        countDeleted += DeleteFilePath(path);
                        countDeleted += DeleteDirectoryPath(path);
                    }
                }

                logger.LogInformation($"{DateTime.Now}: Удалено {countDeleted} фалов программы {software_for_delete.Name}");
            }
            catch(Exception e)
            {
                logger.LogWarning($"{DateTime.Now}: {e.Message}");
            }
        }

        private bool IsSystemPathExist(string path) => File.Exists(path) || Directory.Exists(path);

        private int DeleteFilePath(string filePath)
        {
            int countDeleted = 0;

            if (!File.Exists(filePath))
                return countDeleted;

            DirectoryInfo? parentDirectory = Directory.GetParent(filePath);

            if (parentDirectory == null)
               return countDeleted;

            foreach (FileInfo fileInfo in parentDirectory.GetFiles())
            {
                try
                {
                    File.Delete(fileInfo.FullName);
                    countDeleted++;
                }
                catch(Exception e)
                {
                    logger.LogWarning($"{DateTime.Now}: {e.Message}");
                }
            }

            return countDeleted;
        }

        private int DeleteDirectoryPath(string directoryPath)
        {
            int countDeleted = 0;

            if (!Directory.Exists(directoryPath))
                return countDeleted;

            FileAttributes attr = File.GetAttributes(directoryPath);
            if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                return countDeleted;

            string[] exes = Directory.GetFiles(directoryPath, "*.exe")
                                     .Concat(Directory.GetDirectories(directoryPath)
                                     .SelectMany(dir => Directory.GetFiles(dir, "*.exe")))
                                     .ToArray();

            foreach(string exe_path in exes)
            {
                countDeleted += DeleteFilePath(exe_path);
            }

            return countDeleted;
        }

        private string ExtractPath(string input)
        {
            string fileName = string.Empty;
            string Arguments = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
                throw new Exception("UninstallString cannot be null or white space");

            if (input.StartsWith("\""))
            {
                int endQuote = input.IndexOf("\"", 1);
                if (endQuote > 0)
                {
                    string filePath = input.Substring(0, endQuote + 1);
                    string args = input.Substring(endQuote + 1).Trim();

                    fileName = filePath.Trim('"');
                    Arguments = args;
                }
            }
            else
            {
                string[] parts = input.Split(new char[] { ' ' }, 2);
                fileName = parts[0];
                Arguments = parts.Length > 1 ? parts[1] : string.Empty;
            }

            return fileName;
        }

        protected override RegistryModel? GetSoftwareFromKey(RegistryKey subKey, string subKeyName)
        {
            RegistryModel? findSoftware = base.GetSoftwareFromKey(subKey, subKeyName);

            if (findSoftware != null 
                && findSoftware.DisplayName == software_for_delete.Name
                && findSoftware.Publisher == software_for_delete.Publisher 
                && findSoftware.DisplayVersion == software_for_delete.Version)
            {
                this.findSoftware = findSoftware;
                tokenSource.Cancel();
            }

            return findSoftware;
        }
    }
}
