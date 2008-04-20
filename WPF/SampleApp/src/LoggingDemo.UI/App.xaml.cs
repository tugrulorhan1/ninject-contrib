using System.Windows;
using LoggingDemo.UI.Interceptors;
using Ninject.Core;
using Ninject.Framework.PresentationFoundation;
using Ninject.Integration.LinFu;
using Ninject.Integration.NLog;

namespace LoggingDemo.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [LogMyCalls]
    public partial class App 
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            // KernelContainer.InitializeApplicationWith(this, new LinFuModule(), new NLogModule(), new WpfModule(), new LoggingModule());
            this.InitializeApplicationWith(new LinFuModule(), new NLogModule(), new WpfModule(), new LoggingModule());
            base.OnStartup(e);
        }

    }
}
