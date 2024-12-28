using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Func;
using Assets.Scripts.Entities.Func.Utilits;

using UnityEngine;

public class FireModule : ActivatorBase3D, IActionOtherModule
{
	[SerializeField] private ActionOtherMode _mode;

	public ActionOtherMode Mode
	{
		get => _mode;
		set => _mode = value;
	}

	public override void Execute(SwitcherTriggerContext switcher)
	{
		Execute(switcher);
	}
}
