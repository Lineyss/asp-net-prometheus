using WindowsManager.Contexts;
using WindowsManager.Services;
using WindowsManager.Abstracts;
using Serilog;
using System.IO;
using WindowsManager.Models;

namespace WindowsManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // C:\\Windows\\system32\\WindowsManager\\windows-manager.log
            string logPath = @$"{Environment.SystemDirectory}\\WindowsManager\\windows-manager.log";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath)
                .CreateLogger();

            CreateHostBuilder(args)
                .UseWindowsService()
                .Build()
                .Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((logger) =>
            {
                logger.ClearProviders();
                logger.AddConsole();
                logger.AddSerilog();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<ISoftwareFinder, RegistrySoftwareFinder>();
                services.AddScoped<ISoftwareDelete, DeleteSoftwareFolder>();
                services.AddScoped<ISoftwareDelete, DeleteSoftware>();
                services.AddScoped<IHttpHostWorker, HttpHostWorker>();
                services.AddScoped<SoftwareWorkerContext>();
                services.AddHostedService<MainService>();
            }); 
    }
}
