using Mono.Cecil;
using UnityEngine;

public class teleporter : MonoBehaviour
{
	public Transform target;
	public float RotationX;
	public float RotationY;
	private float ActualRotY;
	public float RotationZ;
	public float Rotw;
	public Transform Player;

	private void OnTriggerEnter(Collider other)
	{
		ActualRotY = Player.transform.rotation.y + RotationY;
        Player.transform.position = target.transform.position;
        Player.transform.Rotate(RotationX, ActualRotY, RotationZ);
	}
}
