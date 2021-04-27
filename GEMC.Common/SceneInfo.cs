using Castle.MicroKernel.SubSystems.Conversion;

namespace GEMC.Common
{
    [Convertible]
    public class SceneInfo
    {
        public string Case { get; set; }

        public string ObsSceneName { get; set; }

    }
}