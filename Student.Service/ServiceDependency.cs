using Autofac;
using Student.Repository;
using Student.Service.Contracts;
using Student.Service.Implementations;

namespace Student.Service
{
    public class ServiceDependency : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<RepositoryDependency>();
            builder.RegisterType<SubjectCommandService>().As<ISubjectCommandService>().InstancePerLifetimeScope();
            builder.RegisterType<SubjectQueryService>().As<ISubjectQueryService>().InstancePerLifetimeScope();
        }
        public static void disposeMethod()
        {

        }
    }
}
