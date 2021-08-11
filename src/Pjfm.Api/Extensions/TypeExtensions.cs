using Scrutor;
using System;
using System.Linq;

namespace Pjfm.Api.Extensions
{
    public static class TypeExtensions
    {
        public static ILifetimeSelector AsClosedTypeOf(this IServiceTypeSelector selector, Type closedType)
        {
            return selector.As(t => t
                .GetInterfaces()
                .Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == closedType)
            );
        }
    }
}