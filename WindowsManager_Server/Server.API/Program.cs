using Prometheus;
using Server.DataAccess;
using Server.API.Models;
using Server.API.Extensions;
using Server.Application.Services;
using Server.DataAccess.Repositories;
using Server.Domain.Absctract.Services;
using Server.Domain.Absctract.Repositories;
using Server.API.Services;

namespace Server.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddDbContext<DataBaseContext>();

            builder.Services.AddScoped<IHostService, HostService>();
            builder.Services.AddScoped<ISoftwareService, SoftwareService>();
            builder.Services.AddScoped<IPublisherService, PublisherService>();
            builder.Services.AddScoped<IHostSoftwareService, HostSoftwareService>();

            builder.Services.AddScoped<IHostRepository, HostRepository>();
            builder.Services.AddScoped<ISoftwareRepository, SoftwareRepository>();
            builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
            builder.Services.AddScoped<IHostSoftwareRepository, HostSoftwareRepository>();

            builder.Services.AddScoped<MetricsService>();

            builder.Services.AddHostedService<MetricsBackgroundService>();

            builder.Services.AddAutoMapper(typeof(MapperProfile));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseMetricServer();

            app.ApplyMigrations();

            app.MapControllers();

            app.Run();
        }
    }
}