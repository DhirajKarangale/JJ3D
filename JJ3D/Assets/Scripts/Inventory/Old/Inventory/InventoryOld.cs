using System.Collections.Generic;
using UnityEngine;

public class InventoryOld : MonoBehaviour
{
    public InventoryUIOld inventoryUI;
    [SerializeField] ItemOld itemDefault;
    [SerializeField] Transform player;

    public int space = 20;
    public List<ItemOld> items = new List<ItemOld>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged;

    private void Start()
    {
        Invoke("DefaultItem", 1);
    }

    private void DefaultItem()
    {
        if (!itemDefault) return;
        itemDefault.Pickup();
        itemDefault.Use();
    }

    public bool Add(ItemOld item)
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

    public void Remove(ItemOld item, bool isThrow)
    {
        if (isThrow) ThrowItem(item);
        items.Remove(item);
        onItemChanged?.Invoke();
    }

    private void ThrowItem(ItemOld item)
    {
        item.transform.position = player.transform.position + new Vector3(0, 2, 2);
        item.gameObject.SetActive(true);
    }
}
