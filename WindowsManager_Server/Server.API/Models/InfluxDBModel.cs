using InfluxDB.Client;

namespace Server.API.Models
{
    public class InfluxDBModel
    {
        private readonly string token = "TP6yPb8jDeWv9J0Xaxbek5osrK_MP_KyhbXhUdD4Qe8T7wCWJT_Zl5Du1NzfGDwGPF_KXkCgxAJMYJWK8pZX2Q==";
        private readonly string url = "http://localhost:8086";

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
