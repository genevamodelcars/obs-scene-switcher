using GEMC.Common;

namespace GEMC.ServerHost
{
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Castle.Windsor.Installer;

    public class WindsorConfiguration
    {
        internal static IWindsorContainer Container;

        public static void Register(HttpConfiguration configuration)
        {
            // See http://www.codeproject.com/Articles/710662/Simplest-Possible-ASP-NET-Web-API-Project-that-Imp

            Container = new WindsorContainer();
            Container.Install(FromAssembly.This());
            Container.Install(new LogInstaller());
            Container.Install(new ControllersInstaller());
            Container.Kernel.Resolver.AddSubResolver(new CollectionResolver(Container.Kernel, true));
            Container.Install(Castle.Windsor.Installer.Configuration.FromAppConfig());
            configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(Container));
        }

        public static void Dispose()
        {
            Container.Dispose();
        }
    }
}