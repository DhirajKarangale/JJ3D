using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryUI inventoryUI;
    [SerializeField] Item itemDefault;
    [SerializeField] Transform player;

    public int space = 20;
    public List<Item> items = new List<Item>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged;

    private void Start()
    {
        DefaultItem();
    }

    private void DefaultItem()
    {
        if (!itemDefault) return;
        itemDefault.Pickup();
        itemDefault.Use();
    }

    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not Enough Space");
            return false;
        }

        items.Add(item);
        onItemChanged?.Invoke();

        return true;
    }

    public void Remove(Item item, bool isThrow)
    {
        if (isThrow) ThrowItem(item);
        items.Remove(item);
        onItemChanged?.Invoke();
    }

    private void ThrowItem(Item item)
    {
        item.transform.position = player.transform.position + new Vector3(0, 1, 1);
        item.gameObject.SetActive(true);
    }
}
