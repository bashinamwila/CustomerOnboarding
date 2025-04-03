using CustomerOnboarding.BusinessLibrary.BaseTypes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Multitenancy.Extensions
{
    public static class MultitenancyServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancy<TTenant,TResolver>(this IServiceCollection services)
            where TResolver : class,ITenantResolver<TTenant>
            where TTenant:class,ITenantInfo
        {
            services.AddScoped<ITenantResolver<TTenant>, TResolver>();
            services.AddScoped(sp => sp.GetService<ITenantResolver<TTenant>>()!.Tenant);
            return services;
        }
    }
}
