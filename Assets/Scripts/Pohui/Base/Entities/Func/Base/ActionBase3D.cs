using System.Linq;

using Assets.Scripts.Entities.Func;
using Assets.Scripts.Entities.Func.Utilits;

using UnityEngine;

namespace Assets.Scripts.Entities
{
	public abstract class ActionBase3D : MonoBehaviour
	{
		private IActionOtherModule[] _actionModuleStart;
		private IActionOtherModule[] _actionModuleEnd;

		/// <summary>Инциализирует доп модули для действий.</summary>
		protected void InitialModules()
		{
			var modules = GetComponents<IActionOtherModule>();

			_actionModuleEnd = modules.Where(x => x.Mode == ActionOtherMode.End).ToArray();
			_actionModuleStart = modules.Where(x => x.Mode == ActionOtherMode.Start).ToArray();
		}

		/// <summary>Выполняет доп модули конца действия.</summary>
		/// <param name="switherTriggerContext"></param>
		protected void ExecuteStartModules(SwitcherTriggerContext switherTriggerContext)
		{
			foreach(var item in _actionModuleStart)
			{
				item.Execute(switherTriggerContext);
			}
		}

		/// <summary>Выполняет доп модули начала действия.</summary>
		/// <param name="switherTriggerContext"></param>
		protected void ExecuteEndModules(SwitcherTriggerContext switherTriggerContext)
		{
			foreach(var item in _actionModuleEnd)
			{
				item.Execute(switherTriggerContext);
			}
		}

		public virtual bool Execute(ActivatorBase3D trigger)
		{
			return Execute(trigger, SwitcherTriggerContext.Toggle);
		}

		public virtual bool Execute(ActivatorBase3D trigger, SwitcherTriggerContext context = SwitcherTriggerContext.Toggle)
		{
			return false;
		}
	}
}
