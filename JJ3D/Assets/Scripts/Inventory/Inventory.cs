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

    public List<InventoryItemObj> initialItems = new List<InventoryItemObj>();

    private List<InventoryItem> items;
    private int currDraggedItemIdx;

    // public event Action<int> OnActionRequired;
    public event Action<int> OnBeginDragging;
    public event Action<int, int> OnSwapItems;

    public bool isActive { get { return obj.activeInHierarchy; } }

    private void Start()
    {
        currDraggedItemIdx = -1;
        mouseFollower.Active(false);
        items = new List<InventoryItem>();
        PrepareItems(inventoryData.size);
        SetActions();
        PrepareInventoryData();
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
            UpdateData(item.Key, item.Value.item.Sprite, item.Value.item.Count);
        }
    }

    private void OnClicked(InventoryItem item)
    {
        int index = items.IndexOf(item);
        if (index == -1) return;
        DeselectItems();
        if (items[index].isEmpty) return;
        items[index].Select();
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
            item.Deselect();
        }
    }

    private void SetActions()
    {
        OnBeginDragging += BeginDragging;
        OnSwapItems += SwapItems;
        inventoryData.OnInventoryUpdate += InventoryUpdate;
    }

    private void DeselectItems()
    {
        foreach (InventoryItem item in items)
        {
            item.Deselect();
        }
    }

    private void ResetDragedItem()
    {
        currDraggedItemIdx = -1;
        mouseFollower.Active(false);
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


    public void ButtonActive(bool isActive)
    {
        obj.SetActive(isActive);

        if (!isActive) return;

        ResetDragedItem();
        DeselectItems();
        foreach (var item in inventoryData.GetCurrInventoryState())
        {
            UpdateData(item.Key, item.Value.item.Sprite, item.Value.item.Count);
        }
    }
}
