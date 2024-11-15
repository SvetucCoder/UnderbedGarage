using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public Inventory playerInventory;

	void Awake()
	{
		playerInventory = gameObject.AddComponent<Inventory>();
        playerInventory.DisplayInventory();
        //Item key = new Item("Rabbit Key", "A key that looks like a rabbit's head. If you collect enough, something might happen.", 1, null);
        //playerInventory.AddItem(key);
        //#рыбаки
    }
}
