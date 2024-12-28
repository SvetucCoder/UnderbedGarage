using UnityEngine;

namespace Assets.Scripts.Entities.Func
{
	public class FuncButton : ActivatorBase3D
	{
		bool _enabled = false;
		
		[Header("GUI")]
		[SerializeField] private Texture _texture;
		[SerializeField] private KeyCode _keyCode = KeyCode.F;

		private void Update()
		{
			if(!_enabled) return; 

			if(Input.GetKeyDown(_keyCode))
			{
				Execute();
			}
		}

		private void Start()
		{
			_enabled = false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Player")
			{
				_enabled = true;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if(other.tag == "Player")
			{
				_enabled = false;
			}
		}

		private void OnGUI()
		{
			if(!_enabled) return;

			GUI.DrawTexture(new Rect(Screen.width/2 - 100, Screen.height/2 - 100, 200, 200), _texture);
		}
	}
}
