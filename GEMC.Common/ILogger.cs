using System;

namespace GEMC.Common
{
    public interface ILogger
    {
        bool IsDebugEnabled(Type source);

        void Debug(Type source, string message);

        void DebugFormat(Type source, string message, params object[] args);

        void Info(Type source, string message);

        void InfoFormat(Type source, string message, params object[] args);

        void Warn(Type source, string message);

        void WarnFormat(Type source, string message, params object[] args);

        void Error(Type source, string message);

        void ErrorFormat(Type source, string message, params object[] args);

        void Error(Type source, string message, Exception exception);
    }
}
