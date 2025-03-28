using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DataAccess;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(DataBaseContext dbContext) : ControllerBase
    {
        private readonly DataBaseContext dbContext = dbContext;

        private readonly string token = "TP6yPb8jDeWv9J0Xaxbek5osrK_MP_KyhbXhUdD4Qe8T7wCWJT_Zl5Du1NzfGDwGPF_KXkCgxAJMYJWK8pZX2Q==";
        private readonly string url = "http://influxdb:8086";

        public readonly string bucket = "321";
        public readonly string org = "123";


        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get()
        {
            using var client = new InfluxDBClient(url, token);

            var hostSoftwareCounts = dbContext.Hosts
                .Include(h => h.HostSoftwares)
                .Select(h => new { h.Hostname, Count = h.HostSoftwares != null ? h.HostSoftwares.Count : 0 })
                .ToList();

            foreach (var host in hostSoftwareCounts)
            {
                var point1 = PointData
                    .Measurement("aa_host_software_counts")
                    .Tag("host", host.Hostname)
                    .Field("value", host.Count)
                    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

                using (var writeApi = client.GetWriteApi())
                {
                    writeApi.WritePoint(point1, bucket, org);
                }
            }

            var totalSoftwareCount = dbContext.Softwares.Count();

            var point = PointData
                .Measurement("aa_total_software_count")
                .Field("value", totalSoftwareCount)
            .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WritePoint(point, bucket, org);
            }

            var softwareAddedLastHour = dbContext.HostSoftwares
                .Count(hs => hs.Added >= DateTime.Now);

            point = PointData
                .Measurement("aa_software_added_last_hour")
                .Field("value", softwareAddedLastHour)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WritePoint(point, bucket, org);
            }

            return Ok();
        }
    }
}
