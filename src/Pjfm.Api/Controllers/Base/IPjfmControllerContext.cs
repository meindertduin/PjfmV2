using Microsoft.Extensions.Configuration;
using Pjfm.Common.Authentication;

namespace Pjfm.Api.Controllers.Base
{
    public interface IPjfmControllerContext
    {  
        IPjfmPrincipal? PjfmPrincipal { get; }
        IConfiguration Configuration { get; }
    }
}