using System;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Validation;
using Mindscape.LightSpeed.Linq;

namespace LoggingDemo.UI.Model
{
	public partial class Post : EntityBase
	{
		[Dependent(ValidatePresence = true)]		private int _blogId;
		[ValidatePresence]
		private string _title;
		[ValidatePresence]
		private string _description;
		[ValidatePresence]
		private DateTime _postDate;
		private int _lockVersion;
		private DateTime? _createdOn;
		private DateTime? _updatedOn;
		private DateTime? _deletedOn;
		private readonly EntityHolder<Blog> _blog = new EntityHolder<Blog>();


		public virtual int BlogId
		{
			get { return _blogId; }
			set { Set(ref _blogId, value); }
		}
		public virtual string Title
		{
			get { return _title; }
			set { Set(ref _title, value); }
		}
		public virtual string Description
		{
			get { return _description; }
			set { Set(ref _description, value); }
		}
		public virtual DateTime PostDate
		{
			get { return _postDate; }
			set { Set(ref _postDate, value); }
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
		public virtual Blog Blog
		{
			get { return Get(_blog); }
			set { Set(_blog, value); }
		}


	}
}
