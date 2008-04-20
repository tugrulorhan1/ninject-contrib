using System.Linq;
using Mindscape.LightSpeed.Linq;
using Ninject.Core;

namespace LoggingDemo.UI.Model
{
    [Service(typeof(ILoggingDemoDataContext))]
	public partial class LoggingDemoDataContext
	{
	}
}
