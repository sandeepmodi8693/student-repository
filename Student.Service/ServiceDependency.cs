using Autofac;
using Student.Repository;
using Student.Repository.Contracts;
using Student.Repository.Database;
using Student.Repository.Implementations;
using Student.Service.Contracts;
using Student.Service.Implementations;

namespace Student.Service
{
    public class ServiceDependency : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<RepositoryDependency>();
            builder.RegisterGeneric(typeof(StudentDBRepo<>)).As(typeof(IStudentRepo<>)).InstancePerLifetimeScope();
            builder.RegisterType<StudentContext>().As<IStudentContext>().InstancePerLifetimeScope();
            builder.RegisterType<CommandService>().As<ICommandService>().InstancePerLifetimeScope();
            builder.RegisterType<QueryService>().As<IQueryService>().InstancePerLifetimeScope();
        }
        public static void disposeMethod()
        {

        }
    }
}
