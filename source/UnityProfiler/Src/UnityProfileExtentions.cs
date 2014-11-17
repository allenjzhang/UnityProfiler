
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace UnityProfileLib
{
	public static class UnityProfileExtentions
	{
		/// <summary>
		/// Custom 
		/// </summary>
		/// <typeparam name="TFrom">Interface to registered.</typeparam>
		/// <typeparam name="TTo">Concret class to be associted with the interface</typeparam>
		/// <param name="container">Unity container instance</param>
		/// <param name="injectionMembers">Additional Injection configurations</param>
		/// <returns></returns>
		public static IUnityContainer RegisterTypeWithProfileInterceptor<TFrom, TTo>(this IUnityContainer container, params InjectionMember[] injectionMembers) where TTo : TFrom
		{
			Dictionary<string, bool> methodEnableMap = new Dictionary<string, bool>();
			// Enable Profiling for a method when
			// 1. Interface method marked PerfProfile and enabled
			// 2. Interface method marked PerfProfile and enabled and Enabled All
			// 3. Class marked PerfProfile and enabled and Enabled All
			// 4. Method marked PerfProfile and enabled
			// 5. Runtime configuration override
			// 6. Global kill switch

			ProcessDeclaration(typeof(TFrom), typeof(TTo).Name, methodEnableMap);

			ProcessDeclaration(typeof(TTo), typeof(TTo).Name, methodEnableMap);

			// Update interceptor cache.
			bool oldValue;
			foreach (var item in methodEnableMap)
			{
				if (item.Value)
					ProfileInterceptorBehavior.ProfileSettingCache.AddOrUpdate(item.Key, item.Value, (k, v) => item.Value);
				else
					ProfileInterceptorBehavior.ProfileSettingCache.TryRemove(item.Key, out oldValue);
			}

			// Register with Unity using interceptor if anything is enable
			var enabledItem = from m in methodEnableMap
							  where m.Value == true
							  select m;

			if (enabledItem.Count() > 0)
				return container.RegisterType<TFrom, TTo>(
					new Interceptor<VirtualMethodInterceptor>(),
					new InterceptionBehavior<ProfileInterceptorBehavior>());
			else
				return container.RegisterType(typeof(TFrom), typeof(TTo), null, null, injectionMembers);

		}

		// Validate UnityProfile attributes on the interface, class, methods
		private static void ProcessDeclaration(Type t, string typeName, Dictionary<string, bool> enableMap)
		{
			bool? typeLevelEnabled = null;

			UnityProfileAttribute p = Attribute.GetCustomAttribute(t, typeof(UnityProfileAttribute), true) as UnityProfileAttribute;

			if (p != null)
				typeLevelEnabled = p.Enabled;


			// Iterate thru individual settings
			foreach (MethodInfo m in t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
			{
				string keyName = typeName + "." + m.Name;
				p = Attribute.GetCustomAttribute(m, typeof(UnityProfileAttribute), true) as UnityProfileAttribute;
				if (p != null && m.IsVirtual)
				{
					if (enableMap.ContainsKey(keyName))
						enableMap[keyName] = p.Enabled;
					else
						enableMap.Add(keyName, p.Enabled);
				}
				else if (typeLevelEnabled.HasValue)
					enableMap.Add(keyName, typeLevelEnabled.Value);
			}
		}
	}
}
