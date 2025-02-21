using Server.DataAccess;
using Server.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.API.Services
{
    public class MetricsService(DataBaseContext dbContext)
    {
        private readonly DataBaseContext dbContext = dbContext;

        public async Task UpdateMetricsAsync()
        {

            var hostSoftwareCounts = await dbContext.Hosts
                .Include(h => h.HostSoftwares)
                .Select(h => new { h.Hostname, Count = h.HostSoftwares != null ? h.HostSoftwares.Count : 0 })
                .ToListAsync();

            foreach (var host in hostSoftwareCounts)
            {
                CustomMetrics.HostSoftwareCount
                    .WithLabels(host.Hostname)
                    .Set(host.Count);
            }

            var totalSoftwareCount = await dbContext.Softwares.CountAsync();
            CustomMetrics.TotalSoftwareCount.Set(totalSoftwareCount);

            var softwareAddedLastHour = await dbContext.HostSoftwares
                .CountAsync(hs => hs.Added >= DateTime.Now);
            CustomMetrics.SoftwareAddedLastHour.Set(softwareAddedLastHour);
        }
    }
}
