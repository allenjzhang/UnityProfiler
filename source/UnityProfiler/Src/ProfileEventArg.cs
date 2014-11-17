using System;


namespace UnityProfileLib
{
	public class ProfileEventArg : EventArgs
	{
		/// <summary>
		/// The Current ProfileLogEntry object to be custom serialized.
		/// </summary>
		public ProfileLogEntry Log { get; private set; }

		internal ProfileEventArg(ProfileLogEntry p)
		{
			Log = p;
		}
	}
}
