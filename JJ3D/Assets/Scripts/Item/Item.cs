using UnityEngine;

public class Item : MonoBehaviour
{
    public float interactRadius = 7f;
    public ItemType itemType;

    [Header("Inventory")]
    public string use;
    public Sprite icon = null;

    [Header("Upgradables")]
    public float armorModifire;
    public float damageModifire;
    public float speedModifire;
    public float modifierMultiplier;

    [Header("Health")]
    public float mxHealth;
    public float currHealth;

    [Header("Level")]
    public float cost;
    public int currLevel;
    public float multiplier;

    private Inventory inventory;
    private EquipmentSlot equipmentSlot;
    private EquipementManager equipementManager;

    private void Start()
    {
        equipementManager = GameManager.instance.equipementManager;
        inventory = GameManager.instance.inventory;
        currHealth = mxHealth;

        InvokeRepeating("CheckPos", 10, 10);

        GetSlot();
    }

    private void GetSlot()
    {
        switch (itemType)
        {
            case ItemType.Helmet:
                equipmentSlot = GameManager.instance.helmetSlot;
                break;

            case ItemType.Vest:
                equipmentSlot = GameManager.instance.vestSlot;
                break;

            case ItemType.Shoes:
                equipmentSlot = GameManager.instance.shoesSlot;
                break;

            case ItemType.Weapon:
                equipmentSlot = GameManager.instance.weaponSlot;
                break;
        }
    }

    private void CheckPos()
    {
        if (this.transform.position.y <= -100)
        {
            transform.position = new Vector3(transform.position.x, 20, transform.position.z);
        }
    }

    public virtual void Use()
    {
        if (itemType == ItemType.Food)
        {
            GameManager.instance.playerHealth.Eat(armorModifire, damageModifire, currLevel);
            inventory.inventoryUI.InventoryButton(false);
            Destroy(this.gameObject);
        }
        else
        {
            GameManager.instance.equipementManager.Equip(this);
        }
        inventory.Remove(this, false);
    }

    public void Upgrade()
    {
        armorModifire *= modifierMultiplier;
        speedModifire *= modifierMultiplier;
        damageModifire *= modifierMultiplier;
        mxHealth *= multiplier;
        cost *= multiplier;
        currLevel++;

        currHealth = mxHealth;
    }

    public void Equipped()
    {
        equipmentSlot.EquipItem(this);
    }

    public void DestroyItem()
    {
        if (itemType == ItemType.Food)
        {
            inventory.Remove(this, false);
        }
        else
        {
            equipementManager.UnEquip((int)itemType, false);
            equipmentSlot.RemoveItem(false);
            equipementManager.DestroyItem(this);
        }
    }

    public void RemoveItem()
    {
        equipmentSlot.RemoveItem(true);
    }

    public void Pickup()
    {
        if (inventory.Add(this))
            this.gameObject.SetActive(false);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, interactRadius);
    // }
}
