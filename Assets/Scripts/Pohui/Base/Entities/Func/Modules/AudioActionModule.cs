using Assets.Scripts.Entities.Func.Utilits;
using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Func
{
	[Serializable]
	public enum ActionOtherMode
	{ 
		Start,
		End
	}

	public interface IActionOtherModule
	{
		ActionOtherMode Mode { get; }

		void Execute(SwitcherTriggerContext switcher);
	}

	[RequireComponent(typeof(AudioSource))]
	public class AudioActionModule : MonoBehaviour, IActionOtherModule
	{
		private AudioSource m_AudioSource;

		[SerializeField] private ActionOtherMode m_ActionOtherMode;

		[SerializeField] private AudioClip _soundclose;
		[SerializeField] private AudioClip _soundopen;
		[SerializeField] private AudioClip _soundblock;

		public ActionOtherMode Mode
		{
			get => m_ActionOtherMode; 
			set => m_ActionOtherMode = value;
		}

		public void Start()
		{
			m_AudioSource = GetComponent<AudioSource>();
		}

		public void Execute(SwitcherTriggerContext switcher)
		{
			switch(switcher)
			{ 
				case SwitcherTriggerContext.None:
					break;
				case SwitcherTriggerContext.On:
					m_AudioSource.clip = _soundopen;
					break;
				case SwitcherTriggerContext.Off:
					m_AudioSource.clip = _soundclose;
					break;
			}

			m_AudioSource.Play();
		}
	}
}
