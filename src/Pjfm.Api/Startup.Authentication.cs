using System.Linq;
using System.Threading.Tasks;
using Domain.ApplicationUser;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pjfm.Api.Authentication;
using Pjfm.Infrastructure;

namespace Pjfm.Api
{
    public partial class Startup
    {
        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<PjfmContext>()
                .AddDefaultTokenProviders();

            var connectionString = Configuration.GetConnectionString("ApplicationDb");

            var identityServiceBuilder = services.AddIdentityServer();

            identityServiceBuilder.AddAspNetIdentity<ApplicationUser>();

            if (WebHostEnvironment.IsProduction())
            {
                identityServiceBuilder.AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseSqlServer(
                            new SqlConnection(connectionString),
                            sqlServerDbContextOptionsBuilder =>
                            {
                                sqlServerDbContextOptionsBuilder.EnableRetryOnFailure();
                                sqlServerDbContextOptionsBuilder.MigrationsAssembly("Pjfm.Infrastructure");
                            });
                    })
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(new SqlConnection(connectionString),
                                sqlServerDbContextOptionsBuilder =>
                                {
                                    sqlServerDbContextOptionsBuilder.EnableRetryOnFailure();
                                    sqlServerDbContextOptionsBuilder.MigrationsAssembly("Pjfm.Infrastructure");
                                });
                    });
            }
            else
            {
                identityServiceBuilder
                    .AddInMemoryIdentityResources(ApiIdentityConfiguration.GetIdentityResources())
                    .AddInMemoryClients(ApiIdentityConfiguration.GetClients())
                    .AddInMemoryApiScopes(ApiIdentityConfiguration.GetApiScopes())
                    .AddDeveloperSigningCredential();
            }

            services.AddLocalApiAuthentication();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/user/login";
                config.LogoutPath = "/authentication/logout";

                // return 401 instead of automatically challenging the user
                config.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(WellKnownPolicies.User, builder => { builder.RequireAuthenticatedUser(); });
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using var serviceScore = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

            if (serviceScore == null)
            {
                return;
            }
            
            var identityContext = serviceScore.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            identityContext.Database.Migrate();

            var pjfmContext = serviceScore.ServiceProvider.GetRequiredService<PjfmContext>();
            pjfmContext.Database.Migrate();
            
            if (!identityContext.Clients.Any())
            {
                foreach (var client in ApiIdentityConfiguration.GetClients())
                {
                    identityContext.Clients.Add(client.ToEntity());
                }

                identityContext.SaveChanges();
            }

            if (!identityContext.IdentityResources.Any())
            {
                foreach (var resource in ApiIdentityConfiguration.GetIdentityResources())
                {
                    identityContext.IdentityResources.Add(resource.ToEntity());
                }

                identityContext.SaveChanges();
            }

            if (!identityContext.ApiScopes.Any())
            {
                foreach (var resource in ApiIdentityConfiguration.GetApiScopes())
                {
                    identityContext.ApiScopes.Add(resource.ToEntity());
                }

                identityContext.SaveChanges();
            }
        }
    }
}