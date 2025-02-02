using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.Translator.Tool
{
    internal class Logging
    {
        public static void Init()
        {
            var loggingDir = "./logs";
            if (!Directory.Exists(loggingDir)) { Directory.CreateDirectory(loggingDir); }
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File($"{loggingDir}/log.txt", rollingInterval: RollingInterval.Day, buffered: true)
#if DEBUG
            .WriteTo.Debug(LogEventLevel.Verbose)
#endif
            .CreateLogger();
        }
    }
}
