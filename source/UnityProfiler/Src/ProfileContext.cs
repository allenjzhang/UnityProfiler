
using System;
namespace UnityProfileLib
{
	public class ProfileContext
	{
		/// <summary>
		/// Indicates the current captured call depth. Thread specific. 
		/// </summary>
		[ThreadStatic]
		public static int NestedDepth;

		/// <summary>
		/// The immediate capthred caller method info. Thread specific.
		/// </summary>
		[ThreadStatic]
		public static string CallingMethod;

		// ------------------------------------------
		// Private variable used to transport settings in Capture/Restore.
		private int _nestedDepth;

		private string _parentMethod;
		// ------------------------------------------

		/// <summary>
		/// This method captures the current thread's context data.
		/// </summary>
		/// <returns>Captured Context used for Restore operation.</returns>
		public static ProfileContext Capture()
		{
			var context = new ProfileContext { _nestedDepth = NestedDepth, _parentMethod = CallingMethod };
			return context;
		}

		/// <summary>
		/// This method restores the captured context on the current thread.
		/// </summary>
		/// <param name="context">The context object from another thread used for restore.</param>
		public static void Restore(ProfileContext context)
		{
			NestedDepth = context._nestedDepth;
			CallingMethod = context._parentMethod;
		}

	}
}
