using Server.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Server.API.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using DataBaseContext dbContexts =
                scope.ServiceProvider.GetRequiredService<DataBaseContext>();

            dbContexts.Database.Migrate();
        }
    }
}
