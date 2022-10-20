using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] InventorySlot inventorySlot;
    [SerializeField] Transform content;

    private List<InventorySlot> inventorySlots;

    private void Start()
    {
        inventorySlots = new List<InventorySlot>();
        InitializeInventory();
    }

    public void InitializeInventory()
    {
        for (int i = 0; i < 20; i++)
        {
            InventorySlot slot = Instantiate(inventorySlot, Vector3.zero, Quaternion.identity);
            slot.transform.SetParent(content);
            inventorySlots.Add(slot);
        }
    }
}
