using System;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using ProxyKit;

namespace Pjfm.Bff
{
    public partial class Startup
    {
        private void ConfigureBff(IServiceCollection services)
        {
            services.AddProxy();
            services.AddAccessTokenManagement();

            services.AddControllers();
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            services.Configure<ForwardedHeadersOptions>(opt =>
            {
                opt.ForwardedHeaders = ForwardedHeaders.XForwardedProto;

                opt.KnownNetworks.Clear();
                opt.KnownProxies.Clear();
            });

            services.AddHttpContextAccessor();
            services.AddHttpClient();
        }

        private static void RunApiProxy(IApplicationBuilder config, string apiServiceUrl)
        {
            if (string.IsNullOrEmpty(apiServiceUrl))
            {
                throw new ArgumentNullException(nameof(apiServiceUrl));
            }

            config.RunProxy(async context =>
            {
                var forwardContext = context.ForwardTo(apiServiceUrl).CopyXForwardedHeaders().AddXForwardedHeaders();

                if (context.User?.Identity?.IsAuthenticated == true)
                {
                    var token = await context.GetUserAccessTokenAsync();
                    forwardContext.UpstreamRequest.SetBearerToken(token);
                }

                return await forwardContext.Send();
            });
        }
    }
}