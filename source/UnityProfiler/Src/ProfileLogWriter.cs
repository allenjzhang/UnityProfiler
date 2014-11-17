
using System;
using System.Diagnostics;
namespace UnityProfileLib
{
	/// <summary>
	/// ProfileEventHandler delegate definition
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void ProfileEventHandler(object sender, ProfileEventArg e);

	public static class ProfileLogWriter
	{
		/// <summary>
		/// This event fires BEFORE method invocation on Profile enabled methods/actions
		/// </summary>
		public static event ProfileEventHandler BeforeInvokeProfileEvent;

		/// <summary>
		/// This event fires AFTER method invocation on Profile enabled methods/actions
		/// </summary>
		public static event ProfileEventHandler AfterInvokeProfileEvent;

		static ProfileLogWriter()
		{
			AfterInvokeProfileEvent += new ProfileEventHandler(ProfileLogWriter.DefaultWriter);
		}

		internal static void BeforeInvoke(Object sender, ProfileLogEntry log)
		{
			if (BeforeInvokeProfileEvent != null)
				BeforeInvokeProfileEvent(sender, new ProfileEventArg(log));
		}
		internal static void AfterInvoke(Object sender, ProfileLogEntry log)
		{
			if (AfterInvokeProfileEvent != null)
				AfterInvokeProfileEvent(sender, new ProfileEventArg(log));
		}

		internal static void DefaultWriter(object sender, ProfileEventArg e)
		{
			if (e.Log != null)
				Trace.TraceInformation(e.Log.ToString());
		}
	}
}
