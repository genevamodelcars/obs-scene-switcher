namespace GEMC.Common
{
    public interface ISceneManager
    {
        void Connect();

        void SwitchScene(string newSceneName);

        string GetCurrentScene();

        void Disconnect();
    }
}