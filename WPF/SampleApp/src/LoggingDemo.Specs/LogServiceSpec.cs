using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LoggingDemo.UI.Model;
using LoggingDemo.UI.Services;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Querying;
using Moq;
using Ninject.Core.Logging;
using NSpecify.Framework;
using NSpecify.Framework.Extensions;

namespace LoggingDemo.Specs
{
    [Context (Description = "Describes the behavior of the LogService")]
    public class LogServiceSpec
    {
        private Mock<IRepository> _mockRepository;
        private ILogService _logService;
        private List<ApplicationEvent> _expectedEvents;

        [BeforeEach]
        public void Before()
        {
            _mockRepository = new Mock<IRepository>();

            _logService = new LogService(_mockRepository.Object);

            _expectedEvents = new List<ApplicationEvent>
                                     {
                                         new ApplicationEvent{Level = "Debug", Message = "Message 1"},
                                         new ApplicationEvent{Level = "Debug", Message = "Message 2"},
                                         new ApplicationEvent{Level = "Debug", Message = "Message 3"}
                                     };
        }

        [Specification("It should find all with eventtime in descending order")]
        public void Should_FindAll_WithEventTime_DescendingOrder()
        {
            _mockRepository.Expect(
                r =>r.Find<ApplicationEvent>(
                    It.Is<Query>(q => q.Order.ToString() == Order.By("EventTime").Descending().ToString()))).Returns(_expectedEvents).Verifiable();

            var events = _logService.FindAll();
            VerifyEvents(_expectedEvents, events);
        }

        [Specification("It should find the events when only a level is supplied")]
        public void Should_FindEventsByLevel()
        {
            _mockRepository.Expect(
                r => r.Find<ApplicationEvent>(
                    It.Is<Query>(q => 
                        q.Order.ToString() == Order.By("EventTime").Descending().ToString() && 
                        q.QueryExpression.ToString() == (Entity.Attribute("Level") == "Debug").ToString()
                    ))).Returns(_expectedEvents).Verifiable();

            var events = _logService.FindForDateRangeAndLevel(null, null, "Debug");
            VerifyEvents(_expectedEvents, events);

        }
        
        // more specs should come here

// ReSharper disable SuggestBaseTypeForParameter
        private void VerifyEvents(List<ApplicationEvent> expectedEvents, ObservableCollection<ApplicationEvent> events)
// ReSharper restore SuggestBaseTypeForParameter
        {
            _mockRepository.VerifyAll();

            events.Must<ApplicationEvent>().Not.Be.Empty("There should be elements in the collection");
            events.Count.Must().Be.Equal(expectedEvents.Count);
            events.Must<ApplicationEvent>().Be.EquivalentTo<ApplicationEvent>(expectedEvents);
        }
    }
}