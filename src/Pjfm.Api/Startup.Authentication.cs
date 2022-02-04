using System.Threading.Tasks;
using Domain.ApplicationUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pjfm.Api.Authentication;
using Pjfm.Common.Authentication;
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
                options.AddPolicy(WellKnownPolicies.SpotifyAuthenticatedUser, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireClaim(PjfmClaimTypes.Role, UserRole.SpotifyAuth.ToString());
                });
            });
        }
    }
}