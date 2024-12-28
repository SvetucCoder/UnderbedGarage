using System;
using Assets.Scripts.Entities.Func.Utilits;

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Entities
{

	public class ActivatorBase3D : MonoBehaviour
	{
		#region Activator

		[Header("Unity API")]
		[SerializeField] private UnityEvent _events;

		#endregion


		public virtual void Execute()
		{
			UnityAPI.ExecuteUnityEvent(_events);
		}

		public virtual void Execute(SwitcherTriggerContext context)
		{
			UnityAPI.ExecuteUnityEvent(_events);
		}

	}
}
