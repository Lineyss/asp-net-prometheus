using Microsoft.Win32;
using System.Diagnostics;
using WindowsManager.Abstracts;
using WindowsManager.Models;

namespace WindowsManager.Services
{
    internal class DeleteSoftware(ILogger<DeleteSoftware> logger) : RegistrySoftwareFinder, ISoftwareDelete
    {
        private readonly ILogger<DeleteSoftware> logger = logger;
        private CancellationTokenSource tokenSource;


        protected HttpSoftware deleted_soft;
        protected RegistryModel _findSoftware;
        /// <summary>
        /// Функция удаляет раздел реестра ПО благодаря переданному объекту типа Software
        /// </summary>
        /// <param name="deleted_soft">Объект типа Software, для которого будет производится удаление из реестра</param>
        /// <returns>Возвращает True если раздел реестра с данным ПО не был найден</returns>

        public async Task Delete(HttpSoftware deleted_soft)
        {
            tokenSource = new CancellationTokenSource();

            _findSoftware = null;

            this.deleted_soft = deleted_soft;
            await Find(tokenSource.Token);

            if (_findSoftware == null)
                return;

            string argument = string.Empty;
            string fileName = string.Empty;

            string url = _findSoftware.QuietUninstallString ?? _findSoftware.UninstallString;

            (fileName, argument) = GetFileNameAndArgumentsFromUninstallString(url);

            if (argument == "/X{7FE24458-0796-4428-99C2-9A0F8DAB93CC}")
            {
                string a = "";
            }

            CreateDeleteProcess(argument, fileName);
        }

        protected virtual void CreateDeleteProcess(string argument, string fileName)
        {
            if (string.IsNullOrWhiteSpace(argument) || string.IsNullOrWhiteSpace(fileName))
                return;

            argument = $"{argument} /quiet";

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = argument,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            try
            {
                using (Process process = new())
                {
                        logger.LogInformation($"Запуск: {fileName} с аргументами: {argument}");
                        process.StartInfo = processStartInfo;
                        process.Start();

                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();

                        // Ждем завершения процесса
                        process.WaitForExit();

                        int exitCode = process.ExitCode;

                        if (exitCode != 0)
                        {
                            logger.LogWarning($"{DateTime.Now}: Ошибка при удалении {deleted_soft.Name}. " +
                                $"Код выхода: {exitCode} \n " +
                                $"Вывод: {output} \n" +
                                $"Ошибки: {error}");
                        }
                        else
                        {
                            logger.LogInformation($"{DateTime.Now}: Удаление {deleted_soft.Name} прошло успешно");
                        }
                }
            }
            catch (Exception e)
            {
                logger.LogError($"{DateTime.Now}: Удаление {deleted_soft.Name} - {e.Message}");
            }
        }

        private (string fileName, string Arguments) GetFileNameAndArgumentsFromUninstallString(string uninstallString)
        {
            string fileName = string.Empty;
            string Arguments = string.Empty;

            if (string.IsNullOrWhiteSpace(uninstallString))
                throw new Exception("UninstallString cannot be null or white space");

            if (uninstallString.StartsWith("\""))
            {
                int endQuote = uninstallString.IndexOf("\"", 1);
                if (endQuote > 0)
                {
                    string filePath = uninstallString.Substring(0, endQuote + 1);
                    string args = uninstallString.Substring(endQuote + 1).Trim();

                    fileName = filePath.Trim('"');
                    Arguments = args;
                }
            }
            else
            {
                string[] parts = uninstallString.Split(new char[] { ' ' }, 2);
                fileName = parts[0];
                Arguments = parts.Length > 1 ? parts[1] : string.Empty;
            }

            return (fileName, Arguments);
        }

        protected override RegistryModel? GetSoftwareFromKey(RegistryKey subKey, string subKeyName)
        {
            RegistryModel? findSoftware = base.GetSoftwareFromKey(subKey, subKeyName);

            if (findSoftware != null &&
               findSoftware.DisplayName == deleted_soft.Name &&
               findSoftware.DisplayVersion == deleted_soft.Version &&
               findSoftware.Publisher == deleted_soft.Publisher)
            {
                _findSoftware = findSoftware;
                tokenSource.Cancel();
            }

            return findSoftware;
        }
    }
}
