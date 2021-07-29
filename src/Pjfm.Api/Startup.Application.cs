using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pjfm.Common.Authentication;

namespace Pjfm.Api
{
    public partial class Startup
    {
        private void ConfigureApplicationServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(x =>
                x.GetRequiredService<IHttpContextAccessor>().HttpContext?.User.GetPjfmPrincipal());
        } 
    }
}