using Assets.Scripts.Entities.Func;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AmbientController : MonoBehaviour
{
	private AudioSource _audioSource;
	private float _sec;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void SwitchMusicAndPlay(AudioClip audioClip, float secOffset = 0)
	{
		if(_audioSource?.SetAudio(audioClip) ?? false)
		{
			_audioSource.time = _sec = secOffset;
			_audioSource.Play();
		}
	}
}
