using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pjfm.Bff
{
    public partial class Startup
    {
        private void ConfigureAuthentication(IServiceCollection services)
        {
            var clientId = Configuration.GetValue<string>("ClientId");
            var clientSecret = Configuration.GetValue<string>("ClientSecret");

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "cookies";
                    options.DefaultChallengeScheme = "oidc";
                    options.DefaultSignOutScheme = "oidc";
                })
                .AddCookie("cookies", options =>
                {
                    options.Cookie.Name = "bff";
                    options.Cookie.SameSite = SameSiteMode.Strict;
                }).AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = Configuration.GetValue<string>("BackendUrl");
                    
                    // confidential client using code flow + PKCE + query response mode
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                    options.ResponseType = "code";
                    options.ResponseMode = "query";

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.UsePkce = true;

                    // save access and refresh token to enable automatic lifetime management
                    options.SaveTokens = true;
                    
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("IdentityServerApi");
                    
                    // request refresh token
                    options.Scope.Add("offline_access");
                });
        }
    }
}