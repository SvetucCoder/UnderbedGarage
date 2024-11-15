using UnityEngine;

[System.Serializable]
public class Item
{
	public string itemName;
	public string itemDescription;
	public long itemID;
	public Sprite itemIcon;
	public long count;

	public Item(string name, string desc, long id,long cnt, Sprite icon)
	{
		itemName = name;
        itemDescription = desc;
        itemID = id;
		itemIcon = icon;
		count = cnt;
	}
}