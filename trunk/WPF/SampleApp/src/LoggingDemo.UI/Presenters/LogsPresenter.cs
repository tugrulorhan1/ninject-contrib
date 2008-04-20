using System.Collections.Generic;
using LoggingDemo.UI.Interceptors;
using LoggingDemo.UI.Services;
using LoggingDemo.UI.Views;
using Ninject.Core;
using Ninject.Framework.PresentationFoundation;
using Ninject.Framework.PresentationFoundation.Infrastructure;

namespace LoggingDemo.UI.Presenters
{
    [Service, LogMyCalls]
    public class LogsPresenter : PresenterBase<ILogsView>
    {
        private readonly ILogService _logService;
        private readonly IRandomDataService _randomDataService;

        [Inject]
        public LogsPresenter(ILogService logService, IRandomDataService randomDataService)
        {
            _logService = logService;
            _randomDataService = randomDataService;
        }

        protected override void OnViewConnected(ILogsView view)
        {
            view.LogLevels = _logService.GetLevels() as List<string>;
            SetFindAll();
        }

        [SubscribeToAction(typeof(LogsView), "FilterLogs")]
        public virtual void FilterLogs()
        {
            View.LoggedEvents = _logService.FindForDateRangeAndLevel(View.StartDate, View.EndDate, View.LogLevel);
        }

        [SubscribeToAction(typeof(LogsView), "GenerateLogs")]
        public virtual void GenerateLogs()
        {
            _randomDataService.AddRandomData(5);
            SetFindAll();   
        }

        private void SetFindAll()
        {
            View.LoggedEvents = _logService.FindAll();
        }

    }
}