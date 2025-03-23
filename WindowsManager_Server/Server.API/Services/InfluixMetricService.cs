using Server.DataAccess;
using Server.API.Models;
using Microsoft.EntityFrameworkCore;
using InfluxDB.Client.Writes;
using InfluxDB.Client.Api.Domain;

namespace Server.API.Services
{
    public class InfluxService(DataBaseContext dbContext, InfluxDBModel influxDb)
    {
        private readonly DataBaseContext dbContext = dbContext;
        private readonly InfluxDBModel influxDb = influxDb;

        public async Task UpdateMerics()
        {
            var hostSoftwareCounts = await dbContext.Hosts
                .Include(h => h.HostSoftwares)
                .Select(h => new { h.Hostname, Count = h.HostSoftwares != null ? h.HostSoftwares.Count : 0 })
                .ToListAsync();

            foreach (var host in hostSoftwareCounts)
            {
                var point1 = PointData
                    .Measurement("aahostSoftwareCounts")
                    .Tag("host", host.Hostname)
                    .Field("value", host.Count)
                    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

                await WritePoint(point1);
            }

            var totalSoftwareCount = await dbContext.Softwares.CountAsync();

            var point = PointData
                .Measurement("aaTotalSoftwareCount")
                .Field("value", totalSoftwareCount)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            await WritePoint(point);

            var softwareAddedLastHour = await dbContext.HostSoftwares
                .CountAsync(hs => hs.Added >= DateTime.Now);
            
            point = PointData
                .Measurement("aaSoftwareAddedLastHour")
                .Field("value", softwareAddedLastHour)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            await WritePoint(point);
        }

        private async Task WritePoint(PointData point)
        {
            using (var writeApi = influxDb.client.GetWriteApi())
            {
                writeApi.WritePoint(point, influxDb.bucket, influxDb.org);
            }
        }
    }
}
