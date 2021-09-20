using System;
using Domain.ApplicationUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pjfm.Api.Authentication;
using Pjfm.Api.HostedServices;
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                InitializeDatabase(app);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();
            
            app.UseOpenApi();

            app.UseRouting();
            app.UseCors();

            app.UseWebSockets(new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(60),
            });

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapRazorPages();
            });

            app.UseMiddleware<PlaybackWebsocketMiddleware>();
        }
    }
}