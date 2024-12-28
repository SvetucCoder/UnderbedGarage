using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace Assets.Scripts.Entities
{
	internal static class UnityAPI
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ExecuteUnityEvent(this UnityEvent unityEvent)
		{
			if(unityEvent != null)
			{
				unityEvent.Invoke();
			}
		}
	}
}
