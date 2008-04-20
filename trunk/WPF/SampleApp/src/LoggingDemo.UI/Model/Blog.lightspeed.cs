using System;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Validation;
using Mindscape.LightSpeed.Linq;

namespace LoggingDemo.UI.Model
{
	public partial class Blog : EntityBase
	{
		[ValidatePresence]
		[ValidateUnique]
		private string _name;
		private int _lockVersion;
		private DateTime? _createdOn;
		private DateTime? _updatedOn;
		private DateTime? _deletedOn;
		


		public virtual string Name
		{
			get { return _name; }
			set { Set(ref _name, value); }
		}
		public virtual int LockVersion
		{
			get { return _lockVersion; }
			set { Set(ref _lockVersion, value); }
		}
		public virtual DateTime? CreatedOn
		{
			get { return _createdOn; }
			set { Set(ref _createdOn, value); }
		}
		public virtual DateTime? UpdatedOn
		{
			get { return _updatedOn; }
			set { Set(ref _updatedOn, value); }
		}
		public virtual DateTime? DeletedOn
		{
			get { return _deletedOn; }
			set { Set(ref _deletedOn, value); }
		}
		public virtual EntityCollection<Post> Posts
		{
			get { return Get(_posts); }
		}


	}
}
