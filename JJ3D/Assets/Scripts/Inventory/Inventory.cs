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

    private List<InventoryItem> items;
    private int currDraggedItemIdx;

    // public event Action<int> OnActionRequired;
    public event Action<int> OnBeginDragging;
    public event Action<int, int> OnSwapItems;

    public bool isActive
    {
        get
        {
            return obj.activeInHierarchy;
        }
    }
    private void Start()
    {
        currDraggedItemIdx = -1;
        mouseFollower.Active(false);
        items = new List<InventoryItem>();
        PrepareItems(inventoryData.size);
        SetActions();
        // inventoryData.Initialize();
    }



    private void BeginDragging(int itemIndex)
    {

    }

    private void SwapItems(int itemIndex1, int itemIndex2)
    {

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
        if (index == -1) return;
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

            item.OnClicked += OnClicked;
            item.OnItemBeginDrag += OnBeginDrag;
            item.OnItemEndDrag += OnEndDrag;
            item.OnItemDrop += OnDrop;
        }
    }

    private void SetActions()
    {
        OnBeginDragging += BeginDragging;
        OnSwapItems += SwapItems;
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
