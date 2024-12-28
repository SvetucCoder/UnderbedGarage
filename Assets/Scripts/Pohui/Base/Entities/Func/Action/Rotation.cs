using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Func.Utilits;

using UnityEngine;

public class Rotation : ActionBase3D
{
	[SerializeField] private bool _alive;	
	[SerializeField] private Vector3 _angularSpeedDirection;
	[SerializeField] private float _speed;
	
	public bool Alive
	{
		get => _alive;
		set => _alive = value;
	}

	public Vector3 AngularSpeedDirection 
	{ 
		get => _angularSpeedDirection;
		set => _angularSpeedDirection = value;
	}

	public float Speed
	{ 
		get => _speed;
		set => _speed = value;
	}

	private void FixedUpdate()
	{
		if(_alive)
		{
			transform.Rotate(_angularSpeedDirection * _speed * Time.fixedDeltaTime);
		}
	}

	public override bool Execute(ActivatorBase3D trigger, SwitcherTriggerContext context)
	{
		switch(context)
		{
			case SwitcherTriggerContext.On:
				_alive = true; break;
			case SwitcherTriggerContext.Off:
				_alive = false; break;
			case SwitcherTriggerContext.Toggle:
				_alive = !_alive; break;
			case SwitcherTriggerContext.Wait:
			case SwitcherTriggerContext.Block:
			case SwitcherTriggerContext.Unblock:
				SetRevers(); break;
		}

		return true;
	}

	

	public void SetRevers() => _speed *= -1;
}
