using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button buttonRemove;
    public Button buttonUse;
    public Text txtUse;
    [SerializeField] Inventory inventory;
    [HideInInspector] public Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;

        txtUse.text = item.use;
        icon.sprite = item.icon;
        icon.enabled = true;
        buttonRemove.interactable = true;
        buttonUse.gameObject.SetActive(false);
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        buttonRemove.interactable = false;
    }

    public void UseItem()
    {
        if (item)
        {
            GameManager.instance.ButtonSound();
            buttonUse.gameObject.SetActive(!buttonUse.gameObject.activeInHierarchy);
        }
    }

    public void DesableUseButton()
    {
        buttonUse.gameObject.SetActive(false);
    }

    public void RemoveButton()
    {
        GameManager.instance.ButtonSound();
        DesableUseButton();
        inventory.Remove(item, true);
    }

    public void UseButton()
    {
        GameManager.instance.ButtonSound();
        DesableUseButton();
        item.Use();
        inventory.inventoryUI.DesableUseButton();
    }
}
