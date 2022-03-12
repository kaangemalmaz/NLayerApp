using Autofac;
using NLayerApp.Core.Repository;
using NLayerApp.Core.Services;
using NLayerApp.Core.UnitOfWorks;
using NLayerApp.Repository;
using NLayerApp.Repository.Repositories;
using NLayerApp.Repository.UnitofWorks;
using NLayerApp.Service.Mappings.AutoMapper;
using NLayerApp.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayerApp.API.Modules
{
    public class RepoServiceModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            var apiAssembly = Assembly.GetExecutingAssembly(); //apinin bulunduğu assembly
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); //repositorylerin bulunduğu assembly
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile)); //servicelerin bulunduğu assembly


            //InstancePerLifetimeScope = scope
            //InstancePerDependency = transient


            //bu demek oluyor ki verilen assemblylerde repository ile bitenlerin hepsini implemente et.
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).
                Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).
                Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            //builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();
        }

    }
}
