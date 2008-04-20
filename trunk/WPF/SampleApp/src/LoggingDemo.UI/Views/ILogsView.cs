using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LoggingDemo.UI.Model;
using Ninject.Framework.PresentationFoundation;

namespace LoggingDemo.UI.Views
{
    public interface ILogsView : IView
    {
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        string LogLevel { get; set; }
        List<string> LogLevels { get; set; }
        ObservableCollection<ApplicationEvent> LoggedEvents { get; set; }

        event EventHandler GenerateLogs;
        event EventHandler FilterLogs;
    }
}