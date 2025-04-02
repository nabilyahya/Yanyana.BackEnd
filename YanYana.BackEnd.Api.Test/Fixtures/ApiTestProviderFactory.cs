
//using Autofac;
//using Autofac.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Yanyana.BackEnd.Data.Context;

//namespace Yanyana.BackEnd.Api.Test.Fixtures
//{
//    public class ApiTestProviderFactory : IServiceProviderFactory<ContainerBuilder>
//    {
//        private AutofacServiceProviderFactory _wrapped;
//        private IServiceCollection _services;

//        public ApiTestProviderFactory()
//        {
//            _wrapped = new AutofacServiceProviderFactory();
//        }

//        public ContainerBuilder CreateBuilder(IServiceCollection services)
//        {
//            _services = services;
//            var descriptor = services.SingleOrDefault(
//                   d => d.ServiceType == typeof(DbContextOptions<YanDbContext>));

//            if (descriptor != null)
//            {
//                services.Remove(descriptor);
//            }

//            // override services that you want
//            services.AddDbContext<YanDbContext>(options =>
//            {
//                var dbHost = "127.0.0.1,1433";
//                var dbDatabase = "FaunaEnFlora";
//                var dbUsername = "sa";
//                var dbPassword = "NaNClouds!";


//                var connString =
//                    $"Data Source={dbHost};Initial Catalog={dbDatabase};User ID={dbUsername};Password={dbPassword};Trust Server Certificate=true;";

//                options.UseSqlServer(connString, x => x.UseNetTopologySuite()).EnableDetailedErrors();
//            });
     
//            return _wrapped.CreateBuilder(services);
//        }

//        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
//        {
//            var sp = _services.BuildServiceProvider();
//            var filters = sp.GetRequiredService<IEnumerable<IStartupConfigureContainerFilter<ContainerBuilder>>>();

//            foreach (var filter in filters)
//            {
//                filter.ConfigureContainer(b => { })(containerBuilder);
//            }
//            var dbContext = sp.GetRequiredService<YanDbContext>();
//            dbContext.Database.EnsureDeleted();
//            dbContext.Database.Migrate();
//            return _wrapped.CreateServiceProvider(containerBuilder);
//        }
//    }
//}
