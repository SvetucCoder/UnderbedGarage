using System.Collections;
using UnityEngine;

public class MonitorController : MonoBehaviour
{
	int iter = 0;
	private MeshRenderer MeshRenderer;
	private Coroutine _coroutine;
	
	[SerializeField] private bool _alive;
	[SerializeField] private string PathIn;

	[SerializeField] private MonitorFramesBuffer FramesBuffers;

	[Range(0.1f,1000)][SerializeField] private float _frequency = 30f;


	private void Start()
	{
		MeshRenderer = GetComponent<MeshRenderer>();

		if(_alive)
		{
			_coroutine = StartCoroutine(StartFrames());
		}
	}

	private IEnumerator StartFrames()
	{
		while(_alive)
		{
			if(FramesBuffers == null) break;

			MeshRenderer.material.mainTexture = FramesBuffers.Buffers[iter++];
			if(iter >= FramesBuffers.Buffers.Length) iter = 0;

			yield return new WaitForSeconds(1 / _frequency);
		}

		_alive = false;
	}

	public void SetBuffers(MonitorFramesBuffer monitorFramesBuffer)
	{
		if(FramesBuffers == monitorFramesBuffer) return;

		StopCoroutine(_coroutine);
		FramesBuffers = monitorFramesBuffer;
		StartCoroutine(StartFrames());
	}
}
