using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryIdentityResources(ApiIdentityConfiguration.GetIdentityResources())
                .AddInMemoryClients(ApiIdentityConfiguration.GetClients())
                .AddInMemoryApiScopes(ApiIdentityConfiguration.GetApiScopes())
                .AddDeveloperSigningCredential();

            services.AddLocalApiAuthentication();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/gebruiker/login";
                config.LoginPath = "/gebruiker/logout";

                // return 401 instead of automatically challenging the user
                config.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(WellKnownPolicies.Gebruiker, builder =>
                {
                    builder.RequireAuthenticatedUser();
                });
            });
        } 
    }
}