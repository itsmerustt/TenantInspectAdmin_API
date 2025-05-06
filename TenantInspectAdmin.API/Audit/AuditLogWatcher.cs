using NLog;
using System.Diagnostics;
using LogLevel = NLog.LogLevel;

namespace TenantInspectAdmin.API.Audit
{
    public class AuditLogWatcher : IDisposable
    {
        private readonly Stopwatch _watch = new Stopwatch();
        private readonly string _description;
        public static Logger _infoLogger = LogManager.GetLogger("infodatabaseLogger");
        public static Logger _fileLogger = LogManager.GetLogger("infoTextLogger");
        private Guid EventId = Guid.NewGuid();
        public AuditLogWatcher(string description)
        {
            _description = description;
            _watch = Stopwatch.StartNew();
        }
        private LogEventInfo CreateLogEventInfo(string description, long timestamp)
        {
            var eventinfo = new LogEventInfo(NLog.LogLevel.Info, null, "");
            string TaskName = description;
            long TimeElapsed = timestamp;
            eventinfo.Properties.Add("EventId", EventId);
            eventinfo.Properties.Add("TaskName", TaskName);
            eventinfo.Properties.Add("TimeElapsed", TimeElapsed);
            return eventinfo;
        }
        public void Dispose()
        {
            _watch.Stop();
            Console.WriteLine($"Task name: {_description}," + $" completed in {_watch.ElapsedMilliseconds / 1000} seconds");
            var timeElapsed = _watch.ElapsedMilliseconds / 1000;
            var eventInfo = CreateLogEventInfo(_description, timeElapsed);
            _fileLogger.Info($"{_description} {timeElapsed} {DateTime.Now}");
            _infoLogger.Log(eventInfo);
        }
    }
}
