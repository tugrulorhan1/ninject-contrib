using System;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Validation;
using Mindscape.LightSpeed.Linq;

namespace LoggingDemo.UI.Model
{
	public partial class ApplicationEvent : EntityBase
	{
		private Guid? _applicationContext;
		private DateTime? _eventTime;
		private string _exception;
		private string _level;
		private string _loggerName;
		private string _message;
		private int? _sequence;
		private string _stackTrace;
		private string _userStackFrame;


		public virtual Guid? ApplicationContext
		{
			get { return _applicationContext; }
			set { Set(ref _applicationContext, value); }
		}
		public virtual DateTime? EventTime
		{
			get { return _eventTime; }
			set { Set(ref _eventTime, value); }
		}
		public virtual string Exception
		{
			get { return _exception; }
			set { Set(ref _exception, value); }
		}
		public virtual string Level
		{
			get { return _level; }
			set { Set(ref _level, value); }
		}
		public virtual string LoggerName
		{
			get { return _loggerName; }
			set { Set(ref _loggerName, value); }
		}
		public virtual string Message
		{
			get { return _message; }
			set { Set(ref _message, value); }
		}
		public virtual int? Sequence
		{
			get { return _sequence; }
			set { Set(ref _sequence, value); }
		}
		public virtual string StackTrace
		{
			get { return _stackTrace; }
			set { Set(ref _stackTrace, value); }
		}
		public virtual string UserStackFrame
		{
			get { return _userStackFrame; }
			set { Set(ref _userStackFrame, value); }
		}


	}
}
