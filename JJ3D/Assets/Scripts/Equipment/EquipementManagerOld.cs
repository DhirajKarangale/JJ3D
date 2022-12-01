using UnityEngine;

public class EquipementManagerOld : MonoBehaviour
{
    public delegate void OnEquipementChanged(ItemOld newItem, ItemOld oldItem);
    public OnEquipementChanged onEquipementChanged;

    [SerializeField] GameObject objHelmet;
    [SerializeField] GameObject objVest;
    [SerializeField] GameObject objShoesLeft;
    [SerializeField] GameObject objShoesRight;
    public GameObject objSwardNormal;
    public GameObject objSwardIce;
    public GameObject objSwardLightning;
    public GameObject objBowNormal;
    public GameObject objBowThree;
    public GameObject objBowFire;
    [SerializeField] GameObject crossHair;

    [HideInInspector] public bool isSwardActive;
    [HideInInspector] public bool isBowActive;

    private GameManager gameManager;
    private InventoryOld inventory;
    private ItemOld[] currEquipments;

    private void Awake()
    {
        objBowNormal.SetActive(false);
        objBowThree.SetActive(false);
        objBowFire.SetActive(false);

        objSwardNormal.SetActive(false);
        objSwardIce.SetActive(false);
        objSwardLightning.SetActive(false);

        crossHair.SetActive(false);
        int noOfSlots = System.Enum.GetNames(typeof(ItemType)).Length;
        currEquipments = new ItemOld[noOfSlots];
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        inventory = gameManager.inventory;
    }

    public void Equip(ItemOld newItem)
    {
        int slotIndex = (int)newItem.itemType;
        ItemOld oldItem = currEquipments[slotIndex];
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

            ItemOld oldItem = currEquipments[slotIndex];

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

    private void ItemStatus(ItemOld item, bool isActive)
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

                objSwardNormal.SetActive(false);
                objSwardIce.SetActive(false);
                objSwardLightning.SetActive(false);

                objBowNormal.SetActive(false);
                objBowThree.SetActive(false);
                objBowFire.SetActive(false);

                crossHair.SetActive(false);

                isSwardActive = false;
                isBowActive = false;

                if (item.transform.name.Contains("SwordNormal"))
                {
                    objSwardNormal.SetActive(isActive);
                    isSwardActive = isActive;
                }
                else if (item.transform.name.Contains("SwordIce"))
                {
                    objSwardIce.SetActive(isActive);
                    isSwardActive = isActive;
                }
                if (item.transform.name.Contains("SwordLightning"))
                {
                    objSwardLightning.SetActive(isActive);
                    isSwardActive = isActive;
                }
                else if (item.transform.name.Contains("BowNormal"))
                {
                    objBowNormal.SetActive(isActive);
                    crossHair.SetActive(isActive);
                    isBowActive = isActive;
                }
                else if (item.transform.name.Contains("BowThree"))
                {
                    objBowThree.SetActive(isActive);
                    crossHair.SetActive(isActive);
                    isBowActive = isActive;
                }
                else if (item.transform.name.Contains("BowFire"))
                {
                    objBowFire.SetActive(isActive);
                    crossHair.SetActive(isActive);
                    isBowActive = isActive;
                }
                break;
        }
    }

    public void DestroyItem(ItemOld item)
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
                    gameManager.DestroyEffect(objSwardNormal.transform.position);
                }
                else if (item.transform.name.Contains("Bow"))
                {
                    gameManager.DestroyEffect(objBowNormal.transform.position);
                }
                break;
        }

        Destroy(item.gameObject);
    }
}