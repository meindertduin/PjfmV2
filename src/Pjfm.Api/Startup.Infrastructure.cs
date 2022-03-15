using System.Data;
using Domain.ApplicationUser;
using Domain.SessionGroup;
using Domain.SpotifyTrack;
using Domain.SpotifyUserData;
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
            services.AddTransient<ISpotifyUserDataRepository, SpotifyUserDataRepository>();
            services.AddTransient<ISpotifyTrackRepository, SpotifyTrackRepository>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<ISessionGroupRepository, SessionGroupRepository>();
            
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

                config.UseOpenIddict();
            });
        }
    }
}