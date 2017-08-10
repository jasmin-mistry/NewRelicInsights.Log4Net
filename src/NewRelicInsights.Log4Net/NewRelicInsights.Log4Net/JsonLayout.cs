using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;

namespace NewRelicInsights.Log4Net
{
    public class JsonLayout : LayoutSkeleton
    {
        private static readonly int ProcessId = Process.GetCurrentProcess().Id;
        private static readonly string MachineName = Environment.MachineName;
        private static readonly string OsVersion = Environment.OSVersion.ToString();
        private static readonly bool Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
        private static readonly bool Is64BitProcess = Environment.Is64BitProcess;

        public override void ActivateOptions()
        {
        }

        public override void Format(TextWriter writer, LoggingEvent e)
        {
            var dic = new Dictionary<string, object>
            {
                ["domain"] = e.Domain,
                ["exceptionObject"] = e.ExceptionObject,
                ["exceptionObjectString"] = e.ExceptionObject == null ? null : e.GetExceptionString(),
                ["identity"] = e.Identity,
                ["is64bitOS"] = Is64BitOperatingSystem,
                ["is64bitProcess"] = Is64BitProcess,
                ["level"] = e.Level.DisplayName,
                ["location"] = e.LocationInformation.FullInfo,
                ["logger"] = e.LoggerName,
                ["machineName"] = MachineName,
                ["messageObject"] = e.MessageObject,
                ["osVersion"] = OsVersion,
                ["pid"] = ProcessId,
                ["renderedMessage"] = e.RenderedMessage,
                ["thread"] = e.ThreadName,
                ["timestampUtc"] = e.TimeStamp.ToUniversalTime().ToString("O"),
                ["userName"] = e.UserName,
                ["workingSet"] = Environment.WorkingSet
            };

            var props = e.GetProperties();

            foreach (var key in props.GetKeys())
            {
                dic.Add(key, props[key]);
            }

            writer.Write(JsonConvert.SerializeObject(dic));
        }

    }
}
