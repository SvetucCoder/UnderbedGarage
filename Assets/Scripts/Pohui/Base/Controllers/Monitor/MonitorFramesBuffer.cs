using UnityEngine;

public class MonitorFramesBuffer : MonoBehaviour
{
	[SerializeField] private Texture2D[] Textures;
	[SerializeField] private string _name;

	public Texture2D[] Buffers => Textures;
}
