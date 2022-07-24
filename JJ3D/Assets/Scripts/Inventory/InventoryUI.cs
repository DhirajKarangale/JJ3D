using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject controlsUI;
    [SerializeField] GameObject bagNotifyObj;
    [SerializeField] Inventory inventory;
    public InventorySlot[] inventorySlots;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        inventory.onItemChanged += UpdateUI;
        bagNotifyObj.SetActive(false);
        InventoryButton(false);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                inventorySlots[i].AddItem(inventory.items[i]);
                bagNotifyObj.SetActive(true);

            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }
    }

    public void DesableUseButton()
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            inventorySlot.DesableUseButton();
        }
    }

    public void InventoryButton(bool isActive)
    {
        gameManager.ButtonSound();
        Time.timeScale = isActive ? 0 : 1;
        bagNotifyObj.SetActive(false);
        inventoryUI.SetActive(isActive);
        controlsUI.SetActive(!isActive);
    }
}
