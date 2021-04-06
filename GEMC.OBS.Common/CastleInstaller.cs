using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace GEMC.OBS.Common
{
    // ReSharper disable once UnusedMember.Global
    public class CastleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<ILogger>()
                    .ImplementedBy<Log4NetLogger>()
                    .LifestyleSingleton());
        }
    }
}