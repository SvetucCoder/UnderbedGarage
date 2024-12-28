using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Func.Utilits;
using UnityEngine;

public class MonitorBuffersChanged : ActionBase3D
{
	public MonitorFramesBuffer Buffers;
	public MonitorController Controller;

	public override bool Execute(ActivatorBase3D trigger)
	{
		if(Controller != null && Buffers != null)
		{
			Controller.SetBuffers(Buffers);
			return true;
		}

		return false;
	}

	public override bool Execute(ActivatorBase3D trigger, SwitcherTriggerContext context = SwitcherTriggerContext.Toggle)
	{
		if(Controller != null && Buffers != null)
		{
			Controller.SetBuffers(Buffers);
			return true;
		}

		return false;
	}
}
