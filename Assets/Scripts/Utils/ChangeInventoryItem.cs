using Fragsurf.Movement;
using UnityEngine;

public class ChangeInventoryItem : MonoBehaviour
{
    public int ID;
    public int count;
    public void add()
    {
        SurfCharacter.Inventory.AddItem(ID, count);
    }
    public void remove()
    {
        SurfCharacter.Inventory.RemoveItem(ID, count);
    }

}
