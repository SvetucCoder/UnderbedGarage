using UnityEngine;

public class AmbientControllerLoader : MonoBehaviour
{
	public AudioClip m_aud;
	public float _timeOffset;

	public AmbientController _ambientController;

	private void Start()
	{
		_ambientController = FindObjectOfType<AmbientController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		_ambientController.SwitchMusicAndPlay(m_aud, _timeOffset);
	}
}
