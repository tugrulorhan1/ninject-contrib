using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LoggingDemo.UI.Model;

namespace LoggingDemo.UI.Services
{
    public interface ILogService : IDataService<ApplicationEvent>
    {
        ObservableCollection<ApplicationEvent> FindForDateRangeAndLevel(DateTime? startDate, DateTime? endDate, string levelName);
        IList<string> GetLevels();
    }
}