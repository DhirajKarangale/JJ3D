using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] InventoryData inventoryData;

    private void OnCollisionEnter(Collision collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();

        if (item)
        {
            int remainder = inventoryData.AddItem(item.InventoryItem, item.Count);
            if (remainder == 0)
            {
                item.DestroyItem();
            }
            else
            {
                item.Count = remainder;
            }
        }
    }
}
