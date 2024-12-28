using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Func.Utilits;

using System.Collections;
using UnityEngine;

public class FuncDoor : ActionBase3D
{
	private Coroutine _coroutine;
	private Vector3 _targetPosition;
	private Vector3 _startPosition;
	private bool _isOpened;

	private IInterpolateAlgs<Vector3> _interpAlgs;

	[Header("Main Options")]
	[SerializeField] private MethodsAction _methodMove;
	[SerializeField] private Vector3 _direct;
	[SerializeField] private float _speed = 5;
	[SerializeField] private bool _toggle = true;
	[SerializeField] private float _timeOpened = 5;

	private void Start()
	{
		_startPosition = transform.position;

		_targetPosition = new Vector3(transform.localScale.x * _direct.x, transform.localScale.y * _direct.y, transform.localScale.z * _direct.z);
		_targetPosition = _targetPosition + transform.position;

		_interpAlgs = VectorInterpolate.CreateAlgs(_methodMove);

		InitialModules();
	}

	private IEnumerator Open()
	{
		ExecuteStartModules(SwitcherTriggerContext.On);

		while(_targetPosition != transform.position)
		{
			MoveFromStartToTarget(transform.position, _targetPosition);
			yield return new WaitForFixedUpdate();
		}
		transform.position = _targetPosition;

		ExecuteEndModules(SwitcherTriggerContext.On);

		if(!_toggle)
		{
			yield return new WaitForSeconds(_timeOpened);

			_isOpened = false;

			ExecuteStartModules(SwitcherTriggerContext.Off);
			while(_startPosition != transform.position)
			{
				MoveFromStartToTarget(transform.position, _startPosition);
				yield return new WaitForFixedUpdate();
			}
			transform.position = _startPosition;

			ExecuteEndModules(SwitcherTriggerContext.Off);
		}
	}

	private IEnumerator Close()
	{
		ExecuteStartModules(SwitcherTriggerContext.Off);

		while(_startPosition != transform.position)
		{
			MoveFromStartToTarget(transform.position, _startPosition);
			yield return new WaitForFixedUpdate();
		}
		transform.position = _startPosition;

		ExecuteEndModules(SwitcherTriggerContext.Off);
	}

	private void MoveFromStartToTarget(Vector3 start, Vector3 end)
	{
		transform.position = _interpAlgs.Interp(start, end, _speed * Time.fixedDeltaTime);
	}

	public override bool Execute(ActivatorBase3D trigger, SwitcherTriggerContext context)
	{
		if(_coroutine != null)
		{
			StopCoroutine(_coroutine);
			_coroutine = null;
		}

		if(context == SwitcherTriggerContext.On)
		{
			if(!_isOpened)
			{
				_isOpened = true;
				_coroutine = StartCoroutine(Open());
				return true;
			}
		}
		else if(context == SwitcherTriggerContext.Off)
		{
			if(_isOpened)
			{
				_isOpened = false;
				_coroutine = StartCoroutine(Close());
				return true;
			}
		}
		else if(context == SwitcherTriggerContext.Toggle)
		{
			if(_isOpened)
			{
				_isOpened = false;
				_coroutine = StartCoroutine(Close());
				return true;
			}
			else
			{
				_isOpened = true;
				_coroutine = StartCoroutine(Open());
			}
		}
		else if(context == SwitcherTriggerContext.Wait)
		{
			_toggle = !_toggle;
		}
		return true;
	}
}