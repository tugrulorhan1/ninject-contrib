using Ninject.Core;
using Ninject.Core.Logging;

using ILightSpeedLogger = Mindscape.LightSpeed.Logging.ILogger;
using INinjectLogger = Ninject.Core.Logging.ILogger;

namespace LoggingDemo.UI.Integration
{
    /// <summary>
    /// This class intercepts logging messages from the LightSpeed context and 
    /// sends them to our NLog logger.
    /// </summary>
    public class LightSpeedLogger : ILightSpeedLogger
    {
        private readonly INinjectLogger logger = NullLogger.Instance;

        public LightSpeedLogger()
        {
        }

        [Inject]
        public LightSpeedLogger(INinjectLogger logger)
        {
            this.logger = logger;
        }

        #region ILogger Members

        public void LogSql(object sql)
        {
            logger.Info(sql.ToString());
        }

        public void LogDebug(object text)
        {
            logger.Debug(text.ToString());
        }

        #endregion
    }
}