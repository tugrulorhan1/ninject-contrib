using System.Linq;
using Mindscape.LightSpeed.Linq;

namespace LoggingDemo.UI.Model
{
	public interface ILoggingDemoDataContext
	{
		IQueryable<ApplicationEvent> ApplicationEvents{ get; }
		IQueryable<Blog> Blogs{ get; }
		IQueryable<Post> Posts{ get; }
	}
}
