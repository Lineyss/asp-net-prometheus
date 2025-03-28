using Server.DataAccess;

namespace Server.API.Services
{
    public class MetricsBackgroundService(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var metricsService = scope.ServiceProvider.GetRequiredService<MetricsService>();
                    await metricsService.UpdateMetricsAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
