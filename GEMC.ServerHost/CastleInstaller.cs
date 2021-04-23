namespace GEMC.ServerHost
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using GEMC.Common;

    public class CastleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<MessageContainer>().LifestyleSingleton());
        }
    }
}