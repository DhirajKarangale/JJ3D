using System;
using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] InventoryData inventoryData;
    [SerializeField] InventorySlot itemPrefab;
    [SerializeField] RectTransform itemContent;
    [SerializeField] DragItem dragItem;

    private List<InventorySlot> inventorySlots;
    private int currDraggedItemIdx;

    public bool isActive { get { return obj.activeInHierarchy; } }

    private void Start()
    {
        currDraggedItemIdx = -1;
        dragItem.Active(false);
        inventorySlots = new List<InventorySlot>();
        PrepareSlots(inventoryData.size);
        PrepareInventoryData();
    }


    private void OnInventoryChange(Dictionary<int, InventoryItem> inventoryState)
    {
        ResetAllItems();
        foreach (var item in inventoryState)
        {
            UpdateItem(item.Key, item.Value.item.itemData.sprite, item.Value.count, item.Value.item.itemData.actionName);
        }
    }

    private void OnClicked(InventorySlot item)
    {
        int index = inventorySlots.IndexOf(item);
        DeselectAllItems(index);
        if ((index == -1) || inventorySlots[index].isEmpty) return;

        if (inventorySlots[index].outline.enabled) inventorySlots[index].Deselect();
        else inventorySlots[index].Select();
    }

    private void OnDragStart(InventorySlot item)
    {
        int index = inventorySlots.IndexOf(item);
        if (index == -1) return;

        InventoryItem itemObj = inventoryData.GetItemAt(index);
        if (itemObj.isEmpty) return;

        currDraggedItemIdx = index;
        DeselectAllItems();
        CreateDragItem(itemObj.item.itemData.sprite, itemObj.count);
    }

    private void OnDragEnd(InventorySlot item)
    {
        ResetDragedItem();
    }

    private void OnDropped(InventorySlot item)
    {
        int index = inventorySlots.IndexOf(item);
        if (currDraggedItemIdx == -1) return;
        SwapItems(currDraggedItemIdx, index);
    }

    private void OnDropButton(InventorySlot item)
    {
        int index = inventorySlots.IndexOf(item);
        if (index == -1) return;

        InventoryItem inventoryItem = inventoryData.GetItemAt(index);
        if (inventoryItem.isEmpty) return;

        DropItem(index, inventoryItem.count);
    }

    private void OnUseButton(InventorySlot item)
    {
        int index = inventorySlots.IndexOf(item);
        if (index == -1) return;

        InventoryItem inventoryItem = inventoryData.GetItemAt(index);
        if (inventoryItem.isEmpty) return;
        inventoryData.RemoveItem(index, 1, null);

        inventoryItem.item.itemData.PerformAction(GameManager.instance.playerStat);
        UpdateUI();
    }


    private void PrepareSlots(int size)
    {
        for (int i = 0; i < size; i++)
        {
            InventorySlot item = Instantiate(itemPrefab);
            item.transform.SetParent(itemContent);
            item.transform.localScale = Vector3.one;
            inventorySlots.Add(item);

            item.OnClicked += OnClicked;
            item.OnDragStart += OnDragStart;
            item.OnDragEnd += OnDragEnd;
            item.OnDropped += OnDropped;
            item.OnDropButton += OnDropButton;
            item.OnUseButton += OnUseButton;
        }
    }

    private void PrepareInventoryData()
    {
        inventoryData.Initialize();
        inventoryData.OnInventoryChange += OnInventoryChange;
    }

    private void DropItem(int index, int amount)
    {
        inventoryData.RemoveItem(index, amount, GameManager.instance.playerStat);
        ResetAllItems();
        // Drop Sound
        UpdateUI();
    }

    private void SwapItems(int itemIndex1, int itemIndex2)
    {
        inventoryData.SwapItems(itemIndex1, itemIndex2);
    }

    private void ResetAllItems()
    {
        foreach (var item in inventorySlots)
        {
            item.Reset();
        }
    }

    private void DeselectAllItems(int leftindex = -1)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i != leftindex) inventorySlots[i].Deselect();
        }
    }

    private void CreateDragItem(Sprite sprite, int count)
    {
        dragItem.Active(true);
        dragItem.SetData(sprite, count);
    }

    private void ResetDragedItem()
    {
        currDraggedItemIdx = -1;
        dragItem.Active(false);
    }

    public void UpdateItem(int index, Sprite sprite, int count, string actionName)
    {
        if (inventorySlots.Count > index)
        {
            inventorySlots[index].SetData(sprite, count, actionName);
        }
    }

    private void UpdateUI()
    {
        ResetDragedItem();
        DeselectAllItems();
        foreach (var item in inventoryData.GetCurrInventoryState())
        {
            UpdateItem(item.Key, item.Value.item.itemData.sprite, item.Value.count, item.Value.item.itemData.actionName);
        }
    }

    public void ButtonActive(bool isActive)
    {
        obj.SetActive(isActive);
        if (!isActive) return;
        UpdateUI();
    }
}
