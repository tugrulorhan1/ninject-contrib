using System;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Validation;
using Mindscape.LightSpeed.Linq;

namespace LoggingDemo.UI.Model
{
	public partial class Blog
	{
        [EagerLoad(AggregateName = "WithPosts")]
        private readonly EntityCollection<Post> _posts = new EntityCollection<Post>();
	}
}
