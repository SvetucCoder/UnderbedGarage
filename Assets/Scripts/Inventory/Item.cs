using UnityEngine;

[System.Serializable]
public class Item
{
	public string itemName;
	public string itemDescription;
	public long itemID;
	public Sprite itemIcon;

	public Item(string name, string desc, long id, Sprite icon)
	{
		itemName = name;
        itemDescription = desc;
        itemID = id;
		itemIcon = icon;
	}
}