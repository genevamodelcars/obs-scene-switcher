using System;
using System.Diagnostics;
using log4net;

namespace GEMC.Common
{
    public class Log4NetLogger : ILogger
    {
        public bool IsDebugEnabled(Type source)
        {
            return LogManager.GetLogger(source).IsDebugEnabled;
        }

        public void Debug(Type source, string message)
        {
            LogManager.GetLogger(source).Debug(message);
        }

        public void DebugFormat(Type source, string message, params object[] args)
        {
            LogManager.GetLogger(source).DebugFormat(message, args);
        }

        public void Info(Type source, string message)
        {
            LogManager.GetLogger(source).Info(message);
        }

        public void InfoFormat(Type source, string message, params object[] args)
        {
            LogManager.GetLogger(source).InfoFormat(message, args);
        }

        public void Warn(Type source, string message)
        {
            LogManager.GetLogger(source).Warn(message);
        }

        public void WarnFormat(Type source, string message, params object[] args)
        {
            LogManager.GetLogger(source).WarnFormat(message, args);
        }

        public void Error(Type source, string message)
        {
            BreakIfDebugging();
            LogManager.GetLogger(source).Error(message);
        }

        public void ErrorFormat(Type source, string message, params object[] args)
        {
            BreakIfDebugging();
            LogManager.GetLogger(source).ErrorFormat(message, args);
        }

        public void Error(Type source, string message, Exception exception)
        {
            BreakIfDebugging();
            LogManager.GetLogger(source).Error(message, exception);
        }

        private static void BreakIfDebugging()
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}