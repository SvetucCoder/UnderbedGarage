using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Func;

using UnityEngine;

public class AddItem : ActivatorBase3D
{
	public Inventory playerInventory;
    public —ursorActivator _cursorActivator;

	[Header("AddItem")]
	public string Name = "Rabbit Key";
	public string Description = "A key that looks like a rabbit's head. If you collect enough, something might happen.";
	public long ID = 1;
	public Sprite Sprite = null;

    private void Start()
    {
        playerInventory = GameObject.FindAnyObjectByType<Inventory>();
        _cursorActivator = GetComponent<—ursorActivator>();

        if (!playerInventory)
            throw new System.Exception("Not found");

        //_cursorActivator.Message = Name;
    }

    //œÓ‰ÓÌÍË.
    public void Add()
	{
        var oldItem = playerInventory.Contain(Name);
        if (oldItem == null)
        {
            Item key = new Item(Name, Description, ID, 1, Sprite);
            playerInventory.AddItem(key);
        }
        else
        {
            oldItem.count++;
        }
    }
}
