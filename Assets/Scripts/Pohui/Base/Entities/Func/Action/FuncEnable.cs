using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Func.Utilits;
using UnityEngine;

public class FuncEnable : ActionBase3D
{
	[SerializeField] private Behaviour _targetComponent;

	public override bool Execute(ActivatorBase3D trigger, SwitcherTriggerContext context)
	{
		switch(context)
		{
			case SwitcherTriggerContext.On:
				_targetComponent.enabled = true;
				return true;

			case SwitcherTriggerContext.Off:
				_targetComponent.enabled = false;
				return true;

			case SwitcherTriggerContext.Toggle: 
				_targetComponent.enabled = !_targetComponent.enabled;
				return true;

		}
		return false;
	}
}
