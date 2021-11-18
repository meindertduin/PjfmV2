using System.Threading.Tasks;
using Domain.ApplicationUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<PjfmContext>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<PjfmContext>();
                })
                .AddServer(options =>
                {
                    options.AllowClientCredentialsFlow();
                    options.AllowAuthorizationCodeFlow()
                        .RequireProofKeyForCodeExchange();
                    
                    options
                        .SetTokenEndpointUris("/connect/authorize")
                        .SetTokenEndpointUris("/connect/token");
                    
                    options
                        .AddEphemeralEncryptionKey()
                        .AddEphemeralSigningKey()
                        .DisableAccessTokenEncryption();

                    options.RegisterScopes("Api");

                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough();
                });

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

            services.AddHostedService<AuthorizationServer>();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using var serviceScore = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

            if (serviceScore == null)
            {
                return;
            }
            
            var pjfmContext = serviceScore.ServiceProvider.GetRequiredService<PjfmContext>();
            pjfmContext.Database.Migrate();
        }
    }
}