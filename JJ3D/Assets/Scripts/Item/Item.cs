using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;

    [Header("Inventory")]
    public Sprite icon = null;

    [Header("Upgradables")]
    public string modifireName;
    public float modifire;

    [Header("Health")]
    public float mxHealth;
    public float currHealth;

    [Header("Level")]
    public float cost;
    public int level;

    private InventoryOld inventory;
    private EquipmentSlot equipmentSlot;
    private EquipementManager equipementManager;

    private void Start()
    {
        equipementManager = GameManager.instance.equipementManager;
        inventory = GameManager.instance.inventory;
        currHealth = mxHealth;

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

    public virtual void Use()
    {
        if (itemType == ItemType.Food)
        {
            GameManager.instance.playerHealth.Eat(modifire, modifireName);
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
        modifire = modifire * 1.5f;
        mxHealth = mxHealth * 1.15f;
        cost = cost * 1.5f;
        level++;

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
