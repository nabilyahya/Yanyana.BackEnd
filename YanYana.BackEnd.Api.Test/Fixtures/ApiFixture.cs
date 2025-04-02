//using Autofac;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Yanyana.BackEnd.Api.Test.Fixtures;

//namespace Yanyana.BackEnd.Api.Test.Fixtures
//{
//    public class ApiFixture : WebApplicationFactory<Program>, IDisposable
//    {
//        protected override IHost CreateHost(IHostBuilder builder)
//        {
//            var x = builder.UseServiceProviderFactory(new ApiTestProviderFactory());
//            return base.CreateHost(x);
//        }
//        protected override void ConfigureWebHost(IWebHostBuilder builder)
//        {
//            SetLocalEnvironmentVariables();
//            builder.UseContentRoot(Directory.GetCurrentDirectory());
//            builder.UseEnvironment("ApiTest");
//            builder.ConfigureAppConfiguration(c =>
//            {
//                c.AddEnvironmentVariables();
//            });
//            builder.ConfigureLogging(logging =>
//            {
//                logging.ClearProviders();
//                logging.AddConsole();
//            });

//            builder.ConfigureTestContainer<ContainerBuilder>(builder =>
//            {
//                builder.RegisterModule(new ApiTestBusinessModule());

//            });
//        }
//        private void SetLocalEnvironmentVariables()
//        {
//            using (var file = File.OpenText("Properties\\launchSettings-test.json"))
//            using (var reader = new JsonTextReader(file))
//            {
//                var jObject = JObject.Load(reader);

//                var variables = jObject
//                    .GetValue("profiles")
//                    .SelectMany(profiles => profiles.Children())
//                    .SelectMany(profile => profile.Children<JProperty>())
//                    .Where(prop => prop.Name == "environmentVariables")
//                    .SelectMany(prop => prop.Value.Children<JProperty>())
//                    .ToList();

//                foreach (var variable in variables)
//                {
//                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
//                }
//            }
//        }
//        //protected override void Dispose(bool disposing)
//        //{
//        //    DummyNotificationManager.MockedService = null;
//        //    DummyStepFunctionManager.MockedService = null;
//        //    DummyMagdaInternalManager.MockedService = null;
//        //    base.Dispose(disposing);
//        //}
//    }
//}
