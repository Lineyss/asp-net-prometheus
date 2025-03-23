using Prometheus;

namespace Server.API.Models
{
    public class CustomMetrics
    {
        public static readonly Gauge HostSoftwareCount = Metrics
        .CreateGauge("AAAAAAAhost_software_count", "Number of software installed on each host.", new GaugeConfiguration
            {
                LabelNames = ["hostname"]
            }
        );

        public static readonly Gauge TotalSoftwareCount = Metrics
            .CreateGauge("AAAAAAAtotal_software_count", "Total number of software in the database.");


        public static readonly Gauge SoftwareAddedLastHour = Metrics
            .CreateGauge("AAAAAAAsoftware_added_last_hour", "Number of software added in the last hour.");
    }
}
