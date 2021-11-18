using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using Pjfm.Infrastructure;

namespace Pjfm.Api.Authentication
{
    public class AuthorizationServer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthorizationServer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<PjfmContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            if (await manager.FindByIdAsync("Bff", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ClientId = "Bff",
                    ClientSecret = "test",
                    DisplayName = "Pjfm",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                    },
                });
            }

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}