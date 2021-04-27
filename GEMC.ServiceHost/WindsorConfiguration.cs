namespace GEMC.ServiceHost
{
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using GEMC.Common;

    public class WindsorConfiguration
    {
        internal static IWindsorContainer Container;

        public static void Register()
        {
            Container = new WindsorContainer();
            Container.Install(FromAssembly.This());
            Container.Install(new LogInstaller());
            Container.Kernel.Resolver.AddSubResolver(new CollectionResolver(Container.Kernel, true));
            Container.Install(Castle.Windsor.Installer.Configuration.FromAppConfig());
        }

        public static void Dispose()
        {
            Container.Dispose();
        }
    }
}