using System;
using System.Collections.Concurrent;

using log4net;

namespace StudentSystem.Common.Logging
{
    public class Log<T> where T : class
    {
        private static ConcurrentDictionary<Type, ILog> Loggers = new ConcurrentDictionary<Type, ILog>();
        
        public static void Info(string message)
        {
            GetLogger(typeof(T)).Info(message);
        }
       
        public static void Error(string message, Exception exception)
        {
            GetLogger(typeof(T)).Error(message, exception);
        }

        private static ILog GetLogger(Type type)
        {
            return Loggers.GetOrAdd(type, t => LogManager.GetLogger(t));
        }
    }
}
