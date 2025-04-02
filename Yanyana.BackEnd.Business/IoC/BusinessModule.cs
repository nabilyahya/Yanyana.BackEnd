using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ANB.WIZapp.Backend.Business.IoC;

public class BusinessModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var services = AddServices(new ServiceCollection());
        builder.Populate(services);
        //builder.RegisterModule<DataModule>();
    }

    public virtual IServiceCollection AddServices(IServiceCollection services)
    {

        //services.AddScoped<INotificationManager, NotificationManager>();
        //services.AddScoped<IIndieningMeldingSchadeManager, IndieningMeldingSchadeManager>();

        return services;
    }
}