using InfluxDB.Client;

namespace Server.API.Models
{
    public class InfluxDBModel
    {
        private readonly string token = "ynv_ukcFxzxjIo4idE9uQNuHhmUKcKg7WDIn5kOT1iXMXz60wXYYNuzrCtR-5lqH8DjRmsy_rME6VSePtqOGDw==";
        private readonly string url = "http://influxdb:8086";

        public readonly string bucket = "321";
        public readonly string org = "123";

        public readonly InfluxDBClient client;
        public InfluxDBModel()
        {
            using var client = new InfluxDBClient(url, token);

            this.client = client;
        }
    }
}
