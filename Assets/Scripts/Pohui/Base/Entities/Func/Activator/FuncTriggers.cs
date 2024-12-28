using Assets.ActionAndActivator;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Func.Utilits;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FuncTriggers : ActivatorBase3D
{
	private bool _onlyOne;

	[SerializeField]
	[Header("���������� ����� ������ ������� � ����")]
	private bool OnWhenEnter;
	[SerializeField]
	[Header("����������� ����� ������ ������� � ����")]
	private bool OffWhenEnter;

	[SerializeField]
	[Header("���������� ����� ������ ������� �� ����")]
	private bool OnWhenExit;
	[SerializeField]
	[Header("����������� ����� ������ ������� �� ����")]
	private bool OffWhenExit;

	[SerializeField]
	[Header("��������� ����������� �������")]
	private bool OffRender;

	[Header("����������� ������ ���� ���.")]
	[SerializeField]
	private bool OnlyOne;

	private void Start()
	{
		this.GetComponents<Collider>()?.Foreach(x => {
			if(!x.isTrigger)
			{
				Debug.LogWarning($"�������� {x.isTrigger} � ��������. ������: {this.name}");
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
