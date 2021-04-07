namespace GEMC.MyRcm.Client
{
    public class SceneInfoEventArgs
    {
        public string Name { get; }
        
        public SceneInfoEventArgs(string name)
        {
            Name = name;
        }
    }
}