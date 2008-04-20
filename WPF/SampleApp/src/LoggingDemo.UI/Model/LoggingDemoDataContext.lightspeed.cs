using System.Linq;
using Mindscape.LightSpeed.Linq;

namespace LoggingDemo.UI.Model
{
	public partial class LoggingDemoDataContext : LightSpeedDataContext, ILoggingDemoDataContext
	{
		public virtual IQueryable<ApplicationEvent> ApplicationEvents
		{
			get { return Query<ApplicationEvent>(); }
		}
		public virtual IQueryable<Blog> Blogs
		{
			get { return Query<Blog>(); }
		}
		public virtual IQueryable<Post> Posts
		{
			get { return Query<Post>(); }
		}
	}
}
