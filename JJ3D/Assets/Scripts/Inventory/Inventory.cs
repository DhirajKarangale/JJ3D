using System;
using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] InventoryData inventoryData;
    [SerializeField] InventoryItem itemPrefab;
    [SerializeField] RectTransform itemContent;
    [SerializeField] MouseFollower mouseFollower;
    [SerializeField] ItemActionPanel actionPanel;

    public List<InventoryItemObj> initialItems = new List<InventoryItemObj>();

    private List<InventoryItem> items;
    private int currDraggedItemIdx;

    public event Action<int> OnActionRequired;
    public event Action<int> OnBeginDragging;
    public event Action<int, int> OnSwapItems;

    public bool isActive { get { return obj.activeInHierarchy; } }

    private void Start()
    {
        actionPanel.Toggle(false);
        currDraggedItemIdx = -1;
        mouseFollower.Active(false);
        items = new List<InventoryItem>();
        PrepareItems(inventoryData.size);
        SetActions();
        PrepareInventoryData();
    }


    private void PerformAction(int index)
    {
        InventoryItemObj inventoryItem = inventoryData.GetItemAt(index);
        if (inventoryItem.isEmpty) return;

        IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
        if (destroyableItem != null)
        {
            inventoryData.RemoveItem(index, 1);
        }

        IItemAction itemAction = inventoryItem.item as IItemAction;
        if (itemAction != null)
        {
            itemAction.PerformAction(GameManager.instance.playerHealth.gameObject, inventoryItem.itemState);
            // Action Sound (Item CLick Sound)    
            if (inventoryData.GetItemAt(index).isEmpty) ResetAllItems();
        }
    }

    // PerformAction Rename from
    public void ActionRequired(int index)
    {
        InventoryItemObj inventoryItem = inventoryData.GetItemAt(index);
        if (inventoryItem.isEmpty) return;

        IItemAction itemAction = inventoryItem.item as IItemAction;
        if (itemAction != null)
        {
            ShowItemAction(index);
            AddAction(itemAction.ActionName, () => PerformAction(index));
        }

        IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
        if (destroyableItem != null)
        {
            AddAction("Drop", () => DropItem(index, inventoryItem.count));
        }
    }

    private void DropItem(int index, int amount)
    {
        inventoryData.RemoveItem(index, amount);
        ResetAllItems();
        // Drop Sound
        UpdateItemUI();
    }

    private void BeginDragging(int itemIndex)
    {
        InventoryItemObj itemObj = inventoryData.GetItemAt(itemIndex);
        if (itemObj.isEmpty) return;
        CreateDragItem(itemObj.item.Sprite, itemObj.count);
    }

    private void SwapItems(int itemIndex1, int itemIndex2)
    {
        inventoryData.SwapItems(itemIndex1, itemIndex2);
    }

    private void InventoryUpdate(Dictionary<int, InventoryItemObj> inventoryState)
    {
        ResetAllItems();
        foreach (var item in inventoryState)
        {
            UpdateData(item.Key, item.Value.item.Sprite, item.Value.count);
        }
    }

    private void OnClicked(InventoryItem item)
    {
        int index = items.IndexOf(item);
        if (index == -1) return;
        DeselectItems();
        if (items[index].isEmpty) return;
        items[index].Select();

        OnActionRequired?.Invoke(index);
    }

    private void OnBeginDrag(InventoryItem item)
    {
        int index = items.IndexOf(item);
        if (index == -1) return;
        currDraggedItemIdx = index;
        OnClicked(item);
        OnBeginDragging?.Invoke(index);
    }

    private void OnEndDrag(InventoryItem item)
    {
        ResetDragedItem();
    }

    private void OnDrop(InventoryItem item)
    {
        int index = items.IndexOf(item);
        if (currDraggedItemIdx == -1) return;
        OnSwapItems?.Invoke(currDraggedItemIdx, index);
    }


    private void PrepareItems(int size)
    {
        for (int i = 0; i < size; i++)
        {
            InventoryItem item = Instantiate(itemPrefab);
            item.transform.SetParent(itemContent);
            item.transform.localScale = Vector3.one;
            items.Add(item);

            item.OnItemDrop += OnDrop;
            item.OnClicked += OnClicked;
            item.OnItemBeginDrag += OnBeginDrag;
            item.OnItemEndDrag += OnEndDrag;
        }
    }

    private void PrepareInventoryData()
    {
        inventoryData.Initialize();
        foreach (InventoryItemObj item in initialItems)
        {
            if (item.isEmpty) continue;
            inventoryData.AddItem(item);
        }
    }

    private void ResetAllItems()
    {
        foreach (var item in items)
        {
            item.Reset();
        }
        DeselectItems();
    }

    private void SetActions()
    {
        OnBeginDragging += BeginDragging;
        OnSwapItems += SwapItems;
        OnActionRequired += ActionRequired;
        inventoryData.OnInventoryUpdate += InventoryUpdate;
    }

    private void DeselectItems()
    {
        foreach (InventoryItem item in items)
        {
            item.Deselect();
        }
        actionPanel.Toggle(false);
    }

    private void ResetDragedItem()
    {
        currDraggedItemIdx = -1;
        mouseFollower.Active(false);
    }

    public void AddAction(string actionName, Action performAction)
    {
        actionPanel.AddButon(actionName, performAction);
    }

    public void ShowItemAction(int index)
    {
        actionPanel.Toggle(true);
        actionPanel.transform.position = items[index].transform.position;
    }



    public void CreateDragItem(Sprite sprite, int count)
    {
        mouseFollower.Active(true);
        mouseFollower.SetData(sprite, count);
    }

    public void UpdateData(int index, Sprite sprite, int count)
    {
        if (items.Count > index)
        {
            items[index].SetData(sprite, count);
        }
    }


    private void UpdateItemUI()
    {
        ResetDragedItem();
        DeselectItems();
        foreach (var item in inventoryData.GetCurrInventoryState())
        {
            UpdateData(item.Key, item.Value.item.Sprite, item.Value.count);
        }
    }

    public void ButtonActive(bool isActive)
    {
        obj.SetActive(isActive);

        if (!isActive) return;

        UpdateItemUI();
    }
}
