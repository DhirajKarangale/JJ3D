using UnityEngine;

public class EquipementManager : MonoBehaviour
{
    public delegate void OnEquipementChanged(Item newItem, Item oldItem);
    public OnEquipementChanged onEquipementChanged;

    [SerializeField] GameObject objHelmet;
    [SerializeField] GameObject objVest;
    [SerializeField] GameObject objShoesLeft;
    [SerializeField] GameObject objShoesRight;
    public GameObject objSward;
    public GameObject objBow;
    [SerializeField] GameObject crossHair;

    private GameManager gameManager;
    private Inventory inventory;
    private Item[] currEquipments;

    private void Awake()
    {
        objBow.SetActive(false);
        objSward.SetActive(false);
        crossHair.SetActive(false);
        int noOfSlots = System.Enum.GetNames(typeof(ItemType)).Length;
        currEquipments = new Item[noOfSlots];
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        inventory = gameManager.inventory;
    }

    public void Equip(Item newItem)
    {
        int slotIndex = (int)newItem.itemType;
        Item oldItem = currEquipments[slotIndex];
        if (oldItem) inventory.Add(oldItem);
        currEquipments[slotIndex] = newItem;
        newItem.Equipped();
        onEquipementChanged?.Invoke(newItem, oldItem);
        ItemStatus(newItem, true);
    }

    public void UnEquip(int slotIndex, bool isThrowItem)
    {
        if (currEquipments[slotIndex])
        {
            ItemStatus(currEquipments[slotIndex], false);

            Item oldItem = currEquipments[slotIndex];

            if (!isThrowItem)
            {
                inventory.Remove(oldItem, isThrowItem);
            }
            else
            {
                if (inventory.items.Count < inventory.space) inventory.Add(oldItem);
                else inventory.Remove(oldItem, isThrowItem);
            }

            currEquipments[slotIndex] = null;

            onEquipementChanged?.Invoke(null, oldItem);
        }
    }

    private void ItemStatus(Item item, bool isActive)
    {
        switch (item.itemType)
        {
            case ItemType.Helmet:
                objHelmet.SetActive(isActive);
                break;
            case ItemType.Vest:
                objVest.SetActive(isActive);
                break;
            case ItemType.Shoes:
                objShoesLeft.SetActive(isActive);
                objShoesRight.SetActive(isActive);
                break;
            case ItemType.Weapon:

                objSward.SetActive(false);
                objBow.SetActive(false);
                crossHair.SetActive(false);

                if (item.transform.name.Contains("Sword"))
                {
                    objSward.SetActive(isActive);
                }
                else if (item.transform.name.Contains("Bow"))
                {
                    objBow.SetActive(isActive);
                    crossHair.SetActive(isActive);
                }
                break;
        }
    }

    public void DestroyItem(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Helmet:
                gameManager.DestroyEffect(objHelmet.transform.position);
                break;
            case ItemType.Vest:
                gameManager.DestroyEffect(objVest.transform.position);
                break;
            case ItemType.Shoes:
                gameManager.DestroyEffect(objShoesLeft.transform.position);
                break;
            case ItemType.Weapon:
                if (item.transform.name.Contains("Sword"))
                {
                    gameManager.DestroyEffect(objSward.transform.position);
                }
                else if (item.transform.name.Contains("Bow"))
                {
                    gameManager.DestroyEffect(objBow.transform.position);
                }
                break;
        }
        item.DestroyItem();
    }
}