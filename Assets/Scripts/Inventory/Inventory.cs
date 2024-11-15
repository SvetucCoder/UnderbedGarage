using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{

    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        items.Add(item);
        Debug.Log("+ " + item.itemName);
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log("- " + item.itemName);
        }
        else
        {
            Debug.Log("Not Found");
        }

    }

    public void DisplayInventory()
    {
        foreach (var item in items)
        {
            Debug.Log("Inventory Item: " + item.itemName);
        }
    }

    public Item Contain(string name) => items.FirstOrDefault(x=>x.itemName == name);
}
