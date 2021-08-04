using System.Data;
using Domain.SpotifyGebruikerData;
using Domain.SpotifyNummer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pjfm.Infrastructure;
using Pjfm.Infrastructure.Repositories;

namespace Pjfm.Api
{
    public partial class Startup
    {
        private void ConfigureInfrastructure(IServiceCollection services)
        {
            services.AddTransient<ISpotifyGebruikersDataRepository, SpotifyGebruikerDataRepository>();
            services.AddTransient<ISpotifyNummerRepository, SpotifyNummerRepository>();
            
            var connectionString = Configuration.GetValue<string>("ConnectionStrings:ApplicationDb");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NoNullAllowedException("Connectionstring of ApplicationDb may not be null or empty");
            }

            services.AddDbContext<PjfmContext>(config =>
            {
                config.UseSqlServer(new SqlConnection(connectionString), builder =>
                {
                    builder.EnableRetryOnFailure();
                    builder.MigrationsAssembly("Pjfm.Infrastructure");
                });
            });
        }
    }
}