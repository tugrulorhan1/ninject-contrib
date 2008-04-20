using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LoggingDemo.UI.Model;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Querying;
using Ninject.Core;
using NLog;

namespace LoggingDemo.UI.Services
{
    [Service(RegisterAs = typeof(ILogService))]
    public class LogService : DataServiceBase<ApplicationEvent>, ILogService
    {
        [Inject]
        public LogService(IRepository repository) : base(repository)
        {
        }

        public override ObservableCollection<ApplicationEvent> FindAll()
        {
            var q = new Query(typeof(ApplicationEvent)) { Order = Order.By("EventTime").Descending() };
            return new ObservableCollection<ApplicationEvent>(_repository.Find<ApplicationEvent>(q) as List<ApplicationEvent>);
        }

        #region ILogService Members

        public ObservableCollection<ApplicationEvent> FindForDateRangeAndLevel(DateTime? startDate, DateTime? endDate, string levelName)
        {
            var q = new Query(typeof (ApplicationEvent)) {Order = Order.By("EventTime").Descending()};

            StartDateExpression(startDate, q);
            EndDateExpression(startDate, q, endDate);
            LevelExpression(startDate, q, endDate, levelName);

            return new ObservableCollection<ApplicationEvent>(_repository.Find<ApplicationEvent>(q) as List<ApplicationEvent>);
        }

        private static void LevelExpression(DateTime? startDate, Query q, DateTime? endDate, string levelName)
        {
            if (string.IsNullOrEmpty(levelName) || levelName.ToUpperInvariant() == "ALL") return;
            if (startDate.HasValue || endDate.HasValue)
                q.QueryExpression = q.QueryExpression && Entity.Attribute("Level") == levelName;
            else
                q.QueryExpression = Entity.Attribute("Level") == levelName;
        }

        private static void EndDateExpression(DateTime? startDate, Query q, DateTime? endDate)
        {
            if (!endDate.HasValue) return;
            if (startDate.HasValue)
                q.QueryExpression = q.QueryExpression && Entity.Attribute("EventTime") <= endDate;
            else
                q.QueryExpression = Entity.Attribute("EventTime") <= endDate;
        }

        private static void StartDateExpression(DateTime? startDate, Query q)
        {
            if (startDate.HasValue)
                q.QueryExpression = Entity.Attribute("EventTime") >= startDate;
        }

        public IList<string> GetLevels()
        {
            return new List<string> {"None", "All", LogLevel.Debug.Name, LogLevel.Info.Name, LogLevel.Warn.Name, LogLevel.Error.Name, LogLevel.Fatal.Name};
        }

        #endregion
    }
}