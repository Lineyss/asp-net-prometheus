using System.Net;
using WindowsManager.Models;
using WindowsManager.Contexts;
using WindowsManager.Abstracts;

namespace WindowsManager
{
    public class MainService(ILogger<MainService> logger, IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly ILogger<MainService> logger = logger;
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory;

        private bool HostnameCreated = false;
        private IHttpHostWorker httpHostWorker;
        private readonly string Hostname = Dns.GetHostName();
        private SoftwareWorkerContext softwareWorkerContext;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            using(IServiceScope serviceScope = serviceScopeFactory.CreateScope())
            {
                httpHostWorker = serviceScope.ServiceProvider.GetRequiredService<IHttpHostWorker>();
                softwareWorkerContext = serviceScope.ServiceProvider.GetRequiredService<SoftwareWorkerContext>();
            }

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CreateHostname(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!HostnameCreated)
                    await CheckValidSoftwares(stoppingToken);
                else
                    HostnameCreated = false;

                logger.LogInformation($"{DateTime.Now}: Ждем следующий проверки...");

                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }

        public async Task CheckValidSoftwares(CancellationToken stoppingToken)
        {
            var systemSoftwares = await GetSoftwares(stoppingToken);
            logger.LogInformation($"{DateTime.Now}: Полученно {systemSoftwares.Count} файлов с компьютера");
            var deleteSoftware = await SendRequestWithRetry(async () =>
                await httpHostWorker.CheckSoftware(Hostname, systemSoftwares, stoppingToken),
                stoppingToken);

            logger.LogInformation($"{DateTime.Now}: Полученно {deleteSoftware.Model?.Count ?? 0} файлов для удаления");

            foreach (HttpSoftware software in deleteSoftware.Model ?? [])
            {
                await softwareWorkerContext.DeleteSoftware(software);
            }
        }

        public async Task CreateHostname(CancellationToken stoppingToken)
        {
            logger.LogInformation($"{DateTime.Now}: Регистрация хоста");

            HttpResult<HostModel> result = await SendRequestWithRetry(async () =>
                await httpHostWorker.CreateHostnameAsync(Hostname, stoppingToken),
                stoppingToken);

            if (result.Status == 200)
            {
                logger.LogInformation($"{DateTime.Now}: Хост {result.Model.Title} зарегестрирован");
                HostnameCreated = true;
                HashSet<HttpSoftware> softwares = await GetSoftwares(stoppingToken);

                logger.LogInformation($"{DateTime.Now}: Регистрация ПО хоста");
                var AddSoftwareStatusCode = await SendRequestWithRetry(async () =>
                    await httpHostWorker.AddSoftwares(Hostname, softwares, stoppingToken),
                    stoppingToken);

                if(AddSoftwareStatusCode.Status == 200)
                {
                    logger.LogInformation($"{DateTime.Now}: ПО хоста зарегестрировано");
                    return;
                }
                else
                {
                    throw new Exception("Ошибка: Не удалось зерегестрировать ПО хост");
                }
            }

            logger.LogInformation($"{DateTime.Now}: Хост был зарегестрирован");
        }

        public async Task<HttpResult<T>> SendRequestWithRetry<T>(Func<Task<HttpResult<T>>> func, CancellationToken token)
        {
            HttpResult<T> result = await func();

            while(result.Status == 500 && !token.IsCancellationRequested)
            {
                logger.LogWarning($"{DateTime.Now}: Не удалось отправить запрос, пробуем еще раз");
                await Task.Delay(TimeSpan.FromHours(1), token);
                result = await func();
            }

            return result;
        }

        public async Task<HashSet<HttpSoftware>> GetSoftwares(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation($"{DateTime.Now}: Получаем файлы");
                HashSet<HttpSoftware> softwares = await softwareWorkerContext.FindSoftware(stoppingToken);
                logger.LogInformation($"{DateTime.Now}: Файлы получены");
                return softwares;
            }
            catch(Exception e)
            {
                logger.LogError($"{DateTime.Now}: GetSoftwares - {e}");
                return new HashSet<HttpSoftware>();
            }
        }
    }
}
