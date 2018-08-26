using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Web.Http;
using WebApi.DAL;
using WebApi.Services;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();
            var configuration = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            RegisterServices(builder);

            var container = builder.Build();

            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<DbFactory>().As<IDbFactory>();
            builder.RegisterType<RequestsXmlSerializer>().As<IRequestsXmlSerializer>();
        }
    }
}
