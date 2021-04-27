using GEMC.MyRcm.Client;

namespace GEMC.ServiceHost
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using GEMC.Common;
    using GEMC.OBS.Client;

    public class CastleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Orchestrator>().LifestyleSingleton(),
                Component.For<MessageContainer>().LifestyleSingleton(),
                Component.For<MessageSender>().LifestyleTransient(),
                Component.For<LiveDataServer>().LifestyleSingleton(),
                Component.For<MessagesListener>().LifestyleSingleton(),
                Component.For<ISceneManager>().ImplementedBy<ObsSceneManager>().LifestyleTransient());

        }
    }
}