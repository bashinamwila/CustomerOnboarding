using CustomerOnboarding.Dal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public static class ConfigurationExtensions
    {
        public static void AddDalMock(this IServiceCollection services)
        {
            services.AddTransient<IStepTypeDal, StepTypeDal>();
        }
    }
}
