using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class InventoryData : ScriptableObject
{
    [SerializeField] internal int size = 10;
    private List<InventoryItem> inventoryItems;
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

    public void AddItem(Item item)
    {
        if (!IsInventoryFull())
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item
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
            dictonary[i] = inventoryItems[i].item.itemData;
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

    public void RemoveItem(int index, Transform playerPos)
    {
        if (inventoryItems.Count > index)
        {
            if (inventoryItems[index].isEmpty) return;
            if (playerPos != null)
            {
                inventoryItems[index].item.ThrowItem(playerPos.position + (playerPos.forward * 3));
            }
            inventoryItems[index] = InventoryItem.GetEmptyItem();
            ChangeInventory();
        }
    }
}

[System.Serializable]
public struct InventoryItem
{
    public Item item;
    public bool isEmpty => item == null;

    public InventoryItem ChangeQuantity(int newCount)
    {
        return new InventoryItem
        {
            item = this.item,
        };
    }

    public static InventoryItem GetEmptyItem()
    {
        return new InventoryItem
        {
            item = null,
        };
    }
}