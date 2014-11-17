
using System.Threading;
using System.Web.Mvc;

namespace UnityProfileLib
{
	public class UnityProfileAttribute : ActionFilterAttribute
	{
		private ProfileLogEntry logEntry = null;

		private string prevParentMethod;

		/// <summary>
		/// Optional. Default: True. 
		/// When True, turn on profiling for scopes based on entities on which it was declarated :
		/// 1. Interface.			All eligible methods in all registered class implementing this interface.
		/// 2. Interface method.	Specific eligible methods in all registered class implementing this interface
		/// 3. Class.				All eligible methods in this specific class.
		/// 4. Class method			Specific eligible methods in this specific class.		
		/// 5. MVCController method Specific MVC Controller method.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// Default constructer that set the default Enabled flag to true.
		/// </summary>
		public UnityProfileAttribute()
		{
			Enabled = true;
		}

		/// <summary>
		/// Default BEFORE Action execution handler which captures entry profile information.
		/// Not publicly utilized.
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			int currentDepth = Interlocked.Increment(ref ProfileContext.NestedDepth);
			prevParentMethod = ProfileContext.CallingMethod;
			string currentMethod = filterContext.Controller.ToString() + "." + filterContext.ActionDescriptor.ActionName;
			logEntry = new ProfileLogEntry()
			{
				NestDepth = currentDepth,
				Module = filterContext.Controller.ToString(),
				CurrentMethod = currentMethod,
				CallingMethod = ProfileContext.CallingMethod
			};
			ProfileContext.CallingMethod = currentMethod;

			ProfileLogWriter.BeforeInvoke(this, logEntry);
		}

		/// <summary>
		/// Default AFTER Action execution handler which captures entry profile information.
		/// Not publicly utilized.
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			logEntry.Complete();
			ProfileLogWriter.AfterInvoke(this, logEntry);
			ProfileContext.CallingMethod = prevParentMethod;
			Interlocked.Decrement(ref ProfileContext.NestedDepth);

		}
	}
}
