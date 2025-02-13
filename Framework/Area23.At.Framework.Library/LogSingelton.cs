using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library
{
    public class LogSingelton
    {
        private static readonly object _lock = new object();
        private static readonly Lazy<LogSingelton> instance = new Lazy<LogSingelton>(() => new LogSingelton());

        public static string LogFile { get; private set; }


        public static LogSingelton StaticAccessor { get => instance.Value; }


        // private readonly ILogger<LogSingelton> _logger;

    }
}
