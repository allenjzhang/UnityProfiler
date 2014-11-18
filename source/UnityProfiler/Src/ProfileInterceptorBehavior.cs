using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Unity.InterceptionExtension;


namespace UnityProfileLib
{
	public class ProfileInterceptorBehavior : IInterceptionBehavior
	{
		internal static ConcurrentDictionary<string, bool> ProfileSettingCache = new ConcurrentDictionary<string, bool>();

		public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
		{
			// Check for Profile setting cache.
			string moduleName = input.Target.GetType().BaseType.Name;
			string methodName = input.MethodBase.Name;
			string currentMethod = string.Format("{0}.{1}", moduleName, methodName);
			IMethodReturn msg = null;
			bool enabledInCache;

			if (ProfileSettingCache.TryGetValue(currentMethod, out enabledInCache) && enabledInCache)
			{
				string prevParentMethod = ProfileContext.CallingMethod;

				try
				{
					int currentDepth = Interlocked.Increment(ref ProfileContext.NestedDepth);
					var logEntry = new ProfileLogEntry()
					{
						NestDepth = currentDepth,
						Module = moduleName,
						CurrentMethod = currentMethod,
						CallingMethod = ProfileContext.CallingMethod
					};
					ProfileContext.CallingMethod = currentMethod;

					ProfileLogWriter.BeforeInvoke(this, logEntry);

					// Main execution of the method to be profiled.
					msg = getNext()(input, getNext);

					logEntry.Complete();
					ProfileLogWriter.AfterInvoke(this, logEntry);
				}
				finally
				{
					ProfileContext.CallingMethod = prevParentMethod;
					Interlocked.Decrement(ref ProfileContext.NestedDepth);
				}

			}
			else
			{
				msg = getNext()(input, getNext);
			}
			return msg;
		}

		public IEnumerable<Type> GetRequiredInterfaces()
		{
			return Type.EmptyTypes;
		}

		public bool WillExecute
		{
			get { return true; }
			set { }
		}
	}
}
