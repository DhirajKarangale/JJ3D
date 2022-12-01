using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class InventoryData : ScriptableObject
{
    [SerializeField] List<InventoryItem> inventoryItems;

    [field: SerializeField]
    public int size { get; set; } = 10;

    public event Action<Dictionary<int, InventoryItem>> OnInventoryChange;

    private void ChangeInventory()
    {
        OnInventoryChange?.Invoke(GetCurrInventoryState());
    }

    // AddNonStackableItem Rename from
    private int AddItemToFirstFreeSlot(Item item, GameObject objItem, int count)
    {
        InventoryItem newItem = new InventoryItem
        {
            item = item,
            count = count
        };

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty)
            {
                inventoryItems[i] = newItem;
                return count;
            }
        }

        return 0;
    }

    private int AddStackableItem(Item item, GameObject objItem, int count)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty) continue;
            if (inventoryItems[i].item.itemData.id == item.itemData.id)
            {
                int amountPossibleToTake = inventoryItems[i].item.itemData.mxStackSize - inventoryItems[i].count;

                if (count > amountPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.itemData.mxStackSize);
                    count -= amountPossibleToTake;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].count + count);
                    ChangeInventory();
                    return 0;
                }
            }
        }

        while ((count > 0) && !IsInventoryFull())
        {
            int newCount = Mathf.Clamp(count, 0, item.itemData.mxStackSize);
            count -= newCount;
            AddItemToFirstFreeSlot(item, objItem, newCount);
        }

        return count;
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

    public void AddItem(Item item, GameObject objItem, int count)
    {
        if (!item.itemData.isStackable)
        {
            while ((count > 0) && !IsInventoryFull())
            {
                count -= AddItemToFirstFreeSlot(item, objItem, 1);
            }
            ChangeInventory();
            // return count;
        }
        count = AddStackableItem(item, objItem, count);
        ChangeInventory();
        // return count;
    }

    public Dictionary<int, InventoryItem> GetCurrInventoryState()
    {
        Dictionary<int, InventoryItem> dictonary = new Dictionary<int, InventoryItem>();
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

    public void RemoveItem(int index, int amount, PlayerStat playerStat)
    {
        if (inventoryItems.Count > index)
        {
            if (inventoryItems[index].isEmpty) return;
            int remainder = inventoryItems[index].count - amount;
            if (remainder <= 0)
            {
                if (playerStat)
                {
                    Debug.Log("name :  " + inventoryItems[index].item.name);
                    inventoryItems[index].item.ThrowItem(playerStat.transform.position);
                }
                inventoryItems[index] = InventoryItem.GetEmptyItem();
            }
            else
            {
                inventoryItems[index] = inventoryItems[index].ChangeQuantity(remainder);
            }
            ChangeInventory();
        }
    }
}

[System.Serializable]
public struct InventoryItem
{
    public int count;
    public Item item;
    public bool isEmpty => item == null;

    public InventoryItem ChangeQuantity(int newCount)
    {
        return new InventoryItem
        {
            item = this.item,
            count = newCount
        };
    }

    public static InventoryItem GetEmptyItem()
    {
        return new InventoryItem
        {
            item = null,
            count = 0,
        };
    }
}