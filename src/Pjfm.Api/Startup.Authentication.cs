using System.Linq;
using System.Threading.Tasks;
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

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
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

            identityServiceBuilder.AddAspNetIdentity<IdentityUser>();

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
                config.LoginPath = "/gebruiker/login";
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
                options.AddPolicy(WellKnownPolicies.Gebruiker, builder => { builder.RequireAuthenticatedUser(); });
            });
        }

        private void InitializeIdentityDatabase(IApplicationBuilder app)
        {
            using var serviceScore = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

            if (serviceScore == null)
            {
                return;
            }
            
            var context = serviceScore.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in ApiIdentityConfiguration.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in ApiIdentityConfiguration.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in ApiIdentityConfiguration.GetApiScopes())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}