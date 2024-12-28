using Assets.ActionAndActivator;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Func.Utilits;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FuncTriggers : ActivatorBase3D
{
	private bool _onlyOne;

	[SerializeField]
	[Header("Включается когда объект заходит в зону")]
	private bool OnWhenEnter;
	[SerializeField]
	[Header("Выключается когда объект заходит в зону")]
	private bool OffWhenEnter;

	[SerializeField]
	[Header("Включается когда объект выходит из зоны")]
	private bool OnWhenExit;
	[SerializeField]
	[Header("Выключается когда объект выходит из зоны")]
	private bool OffWhenExit;

	[SerializeField]
	[Header("Выключает отображение объекта")]
	private bool OffRender;

	[Header("Срабатывает только один раз.")]
	[SerializeField]
	private bool OnlyOne;

	private void Start()
	{
		this.GetComponents<Collider>()?.Foreach(x => {
			if(!x.isTrigger)
			{
				Debug.LogWarning($"Включите {x.isTrigger} в колидере. Объект: {this.name}");
				x.isTrigger = true;
			}
		});

		if(OffRender)
		{
			var mesh = this.GetComponent<Renderer>();
			if(mesh != null)
			{
				mesh.enabled = false;
			}
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		if(_onlyOne) return;

		if(OffWhenEnter && OnWhenEnter)
		{
			Execute(SwitcherTriggerContext.Toggle);
		}
		else
		{
			if(OnWhenEnter)
			{
				Execute(SwitcherTriggerContext.On);
			}
			else if(OffWhenEnter)
			{
				Execute(SwitcherTriggerContext.Off);
			}
			else
			{
				Execute();
			}
		}

		if(OnlyOne)
		{ 
			_onlyOne = true;
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		if(OnlyOne) return;

		if(OnWhenExit && OffWhenExit)
		{
			Execute(SwitcherTriggerContext.Toggle);
		}
		else
		{
			if(OnWhenExit)
			{
				Execute(SwitcherTriggerContext.On);
			}
			if(OffWhenExit)
			{
				Execute(SwitcherTriggerContext.Off);
			}
		}

		if(OnlyOne)
		{
			_onlyOne = true;
		}
	}
}
