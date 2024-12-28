using UnityEngine;

namespace Assets.Scripts.Entities.Func
{
	public static class AudioSwither
	{
		public static bool SetAudio(this AudioSource audioSource, AudioClip clip)
		{
			if(audioSource != null)
			{
				if(clip != audioSource.clip)
				{
					audioSource.clip = clip;
					return true;
				}
			}

			return false;
		}
	}
}
