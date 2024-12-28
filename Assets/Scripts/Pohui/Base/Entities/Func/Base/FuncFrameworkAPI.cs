using System.Runtime.CompilerServices;

using Assets.Scripts.Entities.Func.Utilits;

using UnityEngine;

namespace Assets.Scripts.Entities
{
	internal static class FuncFrameworkAPI
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExecuteFuncAPI(this ActivatorBase3D activator, ActionWithContext[] actionWithContexts)
		{
			foreach(var action in actionWithContexts)
			{
				action.Action.Execute(activator, action.Context);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]

		public static void ExecuteFuncAPI(
			this ActivatorBase3D activator, 
			ActionWithContext[] actionWithContexts,
			SwitcherTriggerContext switcherTriggerContext)
		{
			foreach(var action in actionWithContexts)
			{
				action.Action.Execute(activator, switcherTriggerContext);
			}
		}

#if UNITY_EDITOR_64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InfoDrawDistance(this ActionWithContext[] actions, Vector3 origin)
		{
			if(actions != null)
			{
				foreach(var item in actions)
				{
					if(item == null || item.Action == null) continue;

					item.name = $"{item.Action} : {item.Context}";

					Gizmos.DrawLine(origin, item.Action.transform.position);
				}
			}
		}
#endif
	}
}
