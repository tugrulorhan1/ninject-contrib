using LoggingDemo.UI.Model;
using Mindscape.LightSpeed;
using NLog;

namespace LoggingDemo.UI.Integration
{
    /// <summary>
    /// This class represents a NLog target that we can reference in the nlog.config file
    /// You can use this to use Lightspeed to log to the database just like a file target etc.
    /// </summary>
    [Target("LightSpeedTarget")]
    public class LightSpeedTarget : TargetWithLayout
    {

        public LightSpeedTarget(){
            Layout = "${message}";
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = CompiledLayout.GetFormattedMessage(logEvent);

            var appEvent = new ApplicationEvent
            {
                Sequence = logEvent.SequenceID,
                EventTime = logEvent.TimeStamp,
                Level = logEvent.Level.Name,
                LoggerName = logEvent.LoggerName,
                Message = logMessage
            };

            if (logEvent.Exception != null) appEvent.Exception = logEvent.Exception.ToString();
            if (logEvent.StackTrace != null) appEvent.StackTrace = logEvent.StackTrace.ToString();
            if (logEvent.UserStackFrame != null) appEvent.UserStackFrame = logEvent.UserStackFrame.ToString();

            Repository.Add(appEvent);
            Repository.CompleteUnitOfWork();
        }
    }
}