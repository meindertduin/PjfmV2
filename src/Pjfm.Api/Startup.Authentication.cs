using System;
using System.Threading.Tasks;
using Domain.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
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
            services.AddHostedService<AuthenticationClientsService>();

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

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<PjfmContext>();
                })
                .AddServer(options =>
                {
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();

                    options.SetTokenEndpointUris("/connect/token");
                    options.SetUserinfoEndpointUris("/connect/userinfo");

                    options.UseReferenceAccessTokens();
                    options.UseReferenceRefreshTokens();

                    options.RegisterScopes(OpenIddictConstants.Permissions.Scopes.Email, OpenIddictConstants.Permissions.Scopes.Profile, OpenIddictConstants.Permissions.Scopes.Roles, "api");

                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough();
                }).AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
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

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictConstants.Schemes.Bearer;
                options.DefaultChallengeScheme = OpenIddictConstants.Schemes.Bearer;
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(WellKnownPolicies.User, builder => { builder.RequireAuthenticatedUser(); });
                
                options.AddPolicy(WellKnownPolicies.SpotifyAuthenticatedUser, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireClaim(PjfmClaimTypes.Role, UserRole.SpotifyAuth.ToString());
                });
                
                options.AddPolicy(WellKnownPolicies.Mod, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireClaim(PjfmClaimTypes.Role, UserRole.Mod.ToString());
                });
            });
        }
    }
}