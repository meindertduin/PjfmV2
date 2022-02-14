using System;
using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pjfm.Api.Authentication;
using Pjfm.Api.HostedServices;
using ProxyKit;
using SpotifyPlayback;

namespace Pjfm.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureApplicationServices(services);
            ConfigureInfrastructure(services);
            ConfigureAuthentication(services);
            ConfigurePlayback(services);

            services.AddTransient<PjfmSignInManager>();
            services.AddHostedService<UpdateUserSpotifyTracksHostedService>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            services.AddControllers();
            services.AddRazorPages();
            services.AddSwaggerDocument(options => options.Title = "Pjfm.Api");
            
            // Setup spa application
            services.AddProxy();
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();
            
            app.UseOpenApi();

            app.UseRouting();
            app.UseCors();
            
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseWebSockets(new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(60),
            });

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<PlaybackWebsocketMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapRazorPages();
            });
            
            if (env.IsDevelopment())
            {
                app.Use(async (context, next) =>
                {
                    try
                    {
                        await next();
                    }
                    catch (Exception spaException)
                    {
                        await context.Response.WriteAsync(spaException.Message);
                    }
                });
            }
            
            if (env.IsDevelopment())
            {
                app.MapWhen(p => p.Request.Path.StartsWithSegments("/sockjs-node"),
                    config =>
                    {
                        config.UseSpa(spa => { spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); });
                    });
            }

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
                else
                {
                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                    {
                        OnPrepareResponse = DoNotCache
                    };
                }
            });
        }
        
        private static void DoNotCache(StaticFileResponseContext context)
        {
            context.Context.Request.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            context.Context.Response.Headers.Add("Pragma", "no-cache");
            context.Context.Request.Headers.Add("Expires", "-1");
        }
    }
    
}