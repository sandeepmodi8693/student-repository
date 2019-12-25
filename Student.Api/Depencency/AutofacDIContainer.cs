using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Student.Service;
using System.Web.Http;
using System.Web.Mvc;

namespace Student.Api.Depencency
{
    public class AutofacDIContainer
    {
        public static void ConfigureDIContainer()
        {

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterModule<ServiceDependency>();
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            // Set the dependency resolver for MVC.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}