using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Data Object/Inventory Data")]
public class InventoryData : ScriptableObject
{
    [SerializeField] internal int size = 10;
    internal List<InventoryItem> inventoryItems;
    public event Action<Dictionary<int, ItemData>> OnInventoryChange;

    private void ChangeInventory()
    {
        OnInventoryChange?.Invoke(GetCurrInventoryState());
    }

    private bool IsInventoryFull() => inventoryItems.Where(item => item.isEmpty).Any() == false;
    
    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void AddItem(ItemData itemData)
    {
        if (!IsInventoryFull())
        {
            InventoryItem newItem = new InventoryItem
            {
                itemData = itemData
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    inventoryItems[i] = newItem;
                    break;
                }
            }
        }
        ChangeInventory();
    }

    public Dictionary<int, ItemData> GetCurrInventoryState()
    {
        Dictionary<int, ItemData> dictonary = new Dictionary<int, ItemData>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty)
            {
                continue;
            }
            dictonary[i] = inventoryItems[i].itemData;
        }
        return dictonary;
    }

    public InventoryItem GetItemAt(int index)
    {
        return inventoryItems[index];
    }

    public void SwapItems(int itemIndex1, int itemIndex2)
    {
        InventoryItem item1 = inventoryItems[itemIndex1];
        inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
        inventoryItems[itemIndex2] = item1;
        ChangeInventory();
    }

    public void RemoveItem(int index)
    {
        if (inventoryItems.Count > index)
        {
            if (inventoryItems[index].isEmpty) return;
            inventoryItems[index] = InventoryItem.GetEmptyItem();
            ChangeInventory();
        }
    }
}

[System.Serializable]
public struct InventoryItem
{
    // public Item item;
    public ItemData itemData;
    public bool isEmpty => itemData == null;

    public InventoryItem ChangeQuantity(int newCount)
    {
        return new InventoryItem
        {
            itemData = this.itemData,
        };
    }

    public static InventoryItem GetEmptyItem()
    {
        return new InventoryItem
        {
            itemData = null,
        };
    }
}