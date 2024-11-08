using Assets.Scripts.Entities;
using UnityEngine;

public class AddItem : ActivatorBase3D
{
	public Inventory playerInventory;

	[Header("AddItem")]
	public string Name = "Rabbit Key";
	public string Description = "A key that looks like a rabbit's head. If you collect enough, something might happen.";
	public long ID = 1;
	public Sprite Sprite = null;

	public void FixedUpdate()
	{
		Item key = new Item(Name, Description, ID, Sprite);
	}
}
