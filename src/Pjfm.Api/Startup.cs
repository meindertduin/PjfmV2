using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pjfm.Common.Authentication;
using SpotifyAPI.Web;

namespace Pjfm.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureApplicationServices(services);
            ConfigureInfrastructure(services);
            ConfigureAuthentication(services);
            
            services.AddControllers();
            services.AddRazorPages();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Pjfm.Api", Version = "v1"}); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                InitializeIdentityDatabase(app);
            }
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pjfm.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
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
        }
    }
}