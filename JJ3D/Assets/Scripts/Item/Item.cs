using UnityEngine;

public class Item : MonoBehaviour
{
    public float interactRadius = 3f;
    public EquipmentSlot equipmentSlot;
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
    private EquipementManager equipementManager;

    private void Start()
    {
        equipementManager = GameManager.instance.equipementManager;
        inventory = GameManager.instance.inventory;
        currHealth = mxHealth;
    }

    public virtual void Use()
    {
        if (itemType == ItemType.Food)
        {
            GameManager.instance.playerHealth.Eat(armorModifire, damageModifire, currLevel);
            inventory.inventoryUI.InventoryButton(false);
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

    public void Pickup()
    {
        if (inventory.Add(this))
            this.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
