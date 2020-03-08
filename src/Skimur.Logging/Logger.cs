using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Skimur.Logging
{
 
    public class Logger<T> : Logger, ILogger<T>
    {
        public Logger() : base(typeof(T)) { }
    }

    public class Logger : ILogger
    {
        //private readonly NLog.Logger _log;
        private readonly ILog _log;

        public Logger(Type type)
        {
            // Configure logging
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            _log = LogManager.GetLogger(type);
        }

        //public Logger(Type type) : this(type.Name) { }


        public void Error(string message, Exception ex = null)
        {
            if (ex == null)
                _log.Error(message);
            else
                _log.Error(message, ex);
                //_log.Error(ex, message);
        }

        public void Debug(string message, Exception ex = null)
        {
            if (ex == null)
                _log.Debug(message);
            else
                _log.Debug(message, ex);
                //_log.Debug(ex, message);
        }

        public void Warn(string message, Exception ex = null)
        {
            if (ex == null)
                _log.Warn(message);
            else
                _log.Warn(message, ex);
                //_log.Warn(ex, message);
        }

        public void Info(string message, Exception ex = null)
        {
            if (ex == null)
                _log.Info(message);
            else
                _log.Info(message, ex);
                //_log.Info(ex, message);
        }


        public static ILogger<T> For<T>()
        {
            return new Logger<T>();
        }

        public static ILogger For(Type type)
        {
            return new Logger(type);
        }
    }
}
