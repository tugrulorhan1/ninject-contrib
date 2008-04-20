using System;
using System.Collections.ObjectModel;
using LoggingDemo.UI.Model;
using LoggingDemo.UI.Presenters;
using LoggingDemo.UI.Services;
using LoggingDemo.UI.Views;
using Moq;
using Ninject.Core;
using Ninject.Framework.PresentationFoundation;
using Ninject.Integration.LinFu;
using NSpecify.Framework;
using NSpecify.Framework.Extensions;

namespace LoggingDemo.Specs
{
    [Context]
    public class LogsPresenterSpec
    {
        private Mock<ILogService> _logService;
        private Mock<IRandomDataService> _randomDataService;
        private Mock<ILogsView> _view;
       
        private ObservableCollection<ApplicationEvent> _expectedEvents;
        private InlineModule _inlineModule;

        [BeforeEach]
        public void BeforeEach()
        {
            _logService = new Mock<ILogService>();
            _randomDataService = new Mock<IRandomDataService>();
            _view = new Mock<ILogsView>();

            _expectedEvents = new ObservableCollection<ApplicationEvent>
                                     {
                                         new ApplicationEvent{Level = "Debug", Message = "Message 1"},
                                         new ApplicationEvent{Level = "Debug", Message = "Message 2"},
                                         new ApplicationEvent{Level = "Debug", Message = "Message 3"}
                                     };

            _inlineModule = new InlineModule(im =>
            {
                im.Bind<ILogsView>().ToConstant(_view.Object);
                im.Bind<ILogService>().ToConstant(_logService.Object);
                im.Bind<IRandomDataService>().ToConstant(_randomDataService.Object);
                im.Bind<LogsPresenter>().ToSelf();
            });
        }

        [Specification]
        public void ShouldFilterLogs()
        {
            using (var kernel = new StandardKernel(new LinFuModule(), new WpfModule(), _inlineModule ))
            {
                var presenter = kernel.Get<LogsPresenter>();
                ((IPresenter)presenter).SetView(_view.Object);

                _view.ExpectGet(v => v.StartDate).Returns((DateTime?)null).Verifiable();
                _view.ExpectGet(v => v.EndDate).Returns((DateTime?)null).Verifiable();
                _view.ExpectGet(v => v.LogLevel).Returns("Debug").Verifiable();

                _logService
                    .Expect(ls => ls.FindForDateRangeAndLevel(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.Is<string>(p => p == "Debug")))
                    .Returns(_expectedEvents)
                    .Verifiable();

                _view.ExpectSet(v => v.LoggedEvents).Callback(v => v.Must<ApplicationEvent>().Be.EquivalentTo<ApplicationEvent>(_expectedEvents)).Verifiable();

                presenter.FilterLogs();
                _view.VerifyAll();
                _logService.VerifyAll();
            }
        }

        [Specification]
        public void ShouldGenerateLogs()
        {
            using (var kernel = new StandardKernel(new LinFuModule(), new WpfModule(), _inlineModule))
            {
                var presenter = kernel.Get<LogsPresenter>();
                ((IPresenter) presenter).SetView(_view.Object);


                _randomDataService.Expect(svc => svc.AddRandomData(It.Is<int>(i => i == 5))).Verifiable();
                _logService.Expect(svc => svc.FindAll()).Returns(_expectedEvents).Verifiable();
                _view.ExpectSet(v => v.LoggedEvents).Callback(
                    v => v.Must<ApplicationEvent>().Be.EquivalentTo<ApplicationEvent>(_expectedEvents)).Verifiable();

                presenter.GenerateLogs();

                _randomDataService.VerifyAll();
                _logService.VerifyAll();
                _view.VerifyAll();
            }
        }

        
    }
}