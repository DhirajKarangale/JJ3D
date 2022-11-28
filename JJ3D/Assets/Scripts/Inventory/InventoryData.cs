using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryData : ScriptableObject
{
    [SerializeField] List<InventoryItemObj> inventoryItems;

    [field: SerializeField]
    public int size { get; set; } = 10;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItemObj>();
        for (int i = 0; i < size; i++)
        {
            inventoryItems.Add(InventoryItemObj.GetEmptyItem());
        }
    }

    public void AddItem(InventoryItemData item, int count)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty)
            {
                inventoryItems[i] = new InventoryItemObj
                {
                    item = item,
                    count = count,
                };
            }
        }
    }

    public Dictionary<int, InventoryItemObj> GetCurrInventoryState()
    {
        Dictionary<int, InventoryItemObj> dictonary = new Dictionary<int, InventoryItemObj>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty)
            {
                continue;
            }
            dictonary[i] = inventoryItems[i];
        }
        return dictonary;
    }

    public InventoryItemObj GetItemAt(int index)
    {
        return inventoryItems[index];
    }
}

[System.Serializable]
public struct InventoryItemObj
{
    public int count;
    public InventoryItemData item;
    public bool isEmpty => item == null;

    public InventoryItemObj ChangeQuantity(int newCount)
    {
        return new InventoryItemObj
        {
            item = this.item,
            count = newCount,
        };
    }

    public static InventoryItemObj GetEmptyItem()
    {
        return new InventoryItemObj
        {
            item = null,
            count = 0,
        };
    }
}