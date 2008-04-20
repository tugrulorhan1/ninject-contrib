using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using LoggingDemo.UI.Model;
using LoggingDemo.UI.Presenters;
using Ninject.Framework.PresentationFoundation;
using Ninject.Framework.PresentationFoundation.Infrastructure;

namespace LoggingDemo.UI.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    [PresentedBy(typeof(LogsPresenter))]
    public partial class LogsView :  ILogsView
    {

        public LogsView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            
            this.WireUp(); // KernelContainer.WireUp(this);
        }

        
        public DateTime? StartDate
        {
            get { return startDatePicker.Value; }
            set { startDatePicker.Value = value; }
        }

        public DateTime? EndDate
        {
            get { return endDatePicker.Value; }
            set { endDatePicker.Value = value; }
        }

        public string LogLevel
        {
            get { return logLevels.SelectedValue.ToString(); }
            set { logLevels.SelectedValue = value; }
        }

        public List<string> LogLevels
        {
            get { return logLevels.DataContext as List<string>; }
            set { logLevels.DataContext = value; }
        }

        public ObservableCollection<ApplicationEvent> LoggedEvents
        {
            get { return EventsList.DataContext as ObservableCollection<ApplicationEvent>; }
            set { EventsList.DataContext = value; }
        }


        [PublishAction]
        public event EventHandler GenerateLogs;

        [PublishAction]
        public event EventHandler FilterLogs;


        private void GenerateLogs_Click(object sender, RoutedEventArgs e)
        {
            if (GenerateLogs != null)
                GenerateLogs(null, EventArgs.Empty);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (FilterLogs != null)
                FilterLogs(null, EventArgs.Empty);
        }

    }
}