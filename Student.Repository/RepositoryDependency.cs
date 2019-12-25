using Autofac;
using Student.Repository.Contracts;
using Student.Repository.Database;
using Student.Repository.Implementations;

namespace Student.Repository
{
    public class RepositoryDependency : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StudentContext>().As<IStudentContext>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(StudentDBRepo<>)).As(typeof(IStudentRepo<>)).InstancePerLifetimeScope();
        }
        public static void disposeMethod()
        {

        }
    }
}
