using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class InventoryData : ScriptableObject
{
    [SerializeField] List<InventoryItemObj> inventoryItems;

    [field: SerializeField]
    public int size { get; set; } = 10;

    public event Action<Dictionary<int, InventoryItemObj>> OnInventoryUpdate;

    private void InformAboutChanges()
    {
        OnInventoryUpdate?.Invoke(GetCurrInventoryState());
    }

    // AddNonStackableItem Rename from
    private int AddItemToFirstFreeSlot(InventoryItemData item, int count)
    {
        InventoryItemObj newItem = new InventoryItemObj
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

    private int AddStackableItem(InventoryItemData item, int count)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty) continue;
            if (inventoryItems[i].item.id == item.id)
            {
                int amountPossibleToTake = inventoryItems[i].item.MxStackSize - inventoryItems[i].count;

                if (count > amountPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MxStackSize);
                    count -= amountPossibleToTake;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].count + count);
                    InformAboutChanges();
                    return 0;
                }
            }
        }

        while ((count > 0) && !IsInventoryFull())
        {
            int newCount = Mathf.Clamp(count, 0, item.MxStackSize);
            count -= newCount;
            AddItemToFirstFreeSlot(item, newCount);
        }

        return count;
    }

    private bool IsInventoryFull() => inventoryItems.Where(item => item.isEmpty).Any() == false;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItemObj>();
        for (int i = 0; i < size; i++)
        {
            inventoryItems.Add(InventoryItemObj.GetEmptyItem());
        }
    }

    public int AddItem(InventoryItemData item, int count)
    {
        if (!item.isStackable)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                while ((count > 0) && !IsInventoryFull())
                {
                    // if (inventoryItems[i].isEmpty)
                    // {
                    //     inventoryItems[i] = new InventoryItemObj
                    //     {
                    //         item = item,
                    //         count = count,
                    //     };
                    //     return;
                    // }
                    count -= AddItemToFirstFreeSlot(item, 1);
                }
                InformAboutChanges();
                return count;
            }
        }
        count = AddStackableItem(item, count);
        InformAboutChanges();
        return count;
    }

    public void AddItem(InventoryItemObj item)
    {
        AddItem(item.item, item.count);
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

    public void SwapItems(int itemIndex1, int itemIndex2)
    {
        InventoryItemObj item1 = inventoryItems[itemIndex1];
        inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
        inventoryItems[itemIndex2] = item1;
        InformAboutChanges();
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