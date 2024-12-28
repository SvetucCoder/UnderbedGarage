using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Func.Utilits;
using Fragsurf.Movement;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class СursorActivator : ActivatorBase3D
{
	private MeshRenderer _renderer;
	private Color _cacheColor;
	private Shader _cacheShared;
	private bool _mouseInZone;

	[Header("Messages -----")]
	[Multiline]
	[SerializeField] private string _message;
	[SerializeField] private GUIStyle _style;
	[SerializeField] private Rect _rect = new Rect(0.5f, 0.5f, 100f, 100f);
	[Header("Activators -----")]
	[SerializeField] private SwitcherTriggerContext _mode;
	[Header("Highlighting -----")]
	[SerializeField] private bool IsSelectable;
	[ColorUsage(true)][SerializeField] private Color _color;
	[SerializeField] private Shader _highlightShader;
	[Header("Listeners -----")]
	[SerializeField] private float _distance = 10;
	[SerializeField] private EqualsMode _equelsMode = EqualsMode.Less;
	[SerializeField] private bool _autoFindPlayer;
	[Tooltip("Слушает объекты и вычисляет условие расстояния.")]

    private EqualsMode _currentdistance = EqualsMode.Less;

    private void Start()
	{
		_renderer = GetComponent<MeshRenderer>();
		_cacheColor = _renderer.material.color;
	}


    private EqualsMode CurrentDistance()
	{
		float distance = Vector3.Distance(SurfCharacter.PlayerPos, transform.position);

		if(distance == _distance)
		{
            return EqualsMode.Equels;
        }
		else if (distance > _distance)
        {
            return EqualsMode.More;
        }
        else
        {
            return EqualsMode.Less;
        }
	}

	private void OnMouseDown()
	{
		if(CurrentDistance() == _equelsMode)
		{
            Execute();
        }
	}

	private void OnMouseEnter()
	{
		if(IsSelectable && CurrentDistance() == _equelsMode)
		{
			_cacheColor              = _renderer.material.color;
			_renderer.material.color = _color;
			_mouseInZone             = true;

			if(_highlightShader != null)
			{
				_cacheShared = _renderer.material.shader;
				_renderer.material.shader = _highlightShader;
			}
		}
	}

	private void OnMouseExit()
	{
		_mouseInZone = false;

		if(IsSelectable)
		{
			if(_highlightShader != null)
			{
				_renderer.material.shader = _cacheShared;
			}
			_renderer.material.color = _cacheColor; 
		}
	}

	private void OnMouseUp()
	{
		
	}

	private void OnGUI()
	{
		if(_mouseInZone)
		{
			if(string.IsNullOrEmpty(_message)) return;

			var rect = new Rect(_rect.x * Screen.width - _rect.width / 2, Screen.height * _rect.y - _rect.height / 2, _rect.width, _rect.height);

			if(!string.IsNullOrEmpty(_style.name))
			{
				GUI.TextField(rect, _message, _style);
			}
			else
			{
				GUI.TextField(rect, _message);
			}
		}
	}
}
