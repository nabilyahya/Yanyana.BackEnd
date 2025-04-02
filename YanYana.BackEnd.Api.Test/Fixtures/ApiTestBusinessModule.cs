
using ANB.WIZapp.Backend.Business.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Yanyana.BackEnd.Api.Test.Fixtures
{
    public class ApiTestBusinessModule : BusinessModule
    {
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            //services.AddScoped<IStepFunctionsManager, DummyStepFunctionManager>();
            //services.AddScoped<INotificationManager, DummyNotificationManager>();
            //services.AddScoped<IMagdaInternalManager, DummyMagdaInternalManager>();

            return services;
        }
    }
}
