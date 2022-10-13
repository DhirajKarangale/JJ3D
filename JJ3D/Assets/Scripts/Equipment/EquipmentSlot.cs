using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Image icon;
    [SerializeField] Slider slider;
    [SerializeField] Text txtCost;
    [SerializeField] Text txtDescription;
    [SerializeField] Text txtUpgrade;
    [SerializeField] Button buttonRemove;
    [SerializeField] Button buttonUpgrade;

    [HideInInspector] public Item equipedItem;
    private EquipementManager equipementManager;
    private GameManager gameManager;
    private float upgradeCost;

    private void Start()
    {
        gameManager = GameManager.instance;
        equipementManager = gameManager.equipementManager;
        if (!equipedItem) RemoveItem(true);
    }

    private void OnEnable()
    {
        if (equipedItem) EquipItem(equipedItem);
        else RemoveItem(true);
    }

    public void EquipItem(Item item)
    {
        equipedItem = item;

        Color iconColor = Color.white;
        iconColor.a = 1;
        icon.color = iconColor;

        icon.sprite = item.icon;
        slider.value = item.currHealth / item.mxHealth;
        txtDescription.text = item.modifireName + " : " + item.modifire.ToString("F2") + "\nLevel : " + item.level.ToString();
        buttonRemove.interactable = true;

        if (item.level < 10)
        {
            if (item.currHealth < item.mxHealth)
            {
                upgradeCost = (int)(item.mxHealth - item.currHealth) * 15;
                txtUpgrade.text = "Heal";
            }
            else
            {
                upgradeCost = item.cost;
                txtUpgrade.text = "Upgrade";
            }
            txtCost.text = upgradeCost.ToString("F0");
            buttonUpgrade.interactable = Msg.instance.coin > upgradeCost;
        }
        else
        {
            buttonUpgrade.interactable = false;
            txtUpgrade.text = "Max";
            txtCost.text = "Max";
        }

        if (gameManager) gameManager.ButtonSound();
    }

    public void RemoveItem(bool isThrowItem)
    {
        Color iconColor = Color.black;
        iconColor.a = 0.5f;
        icon.color = iconColor;

        if (!gameManager)
        {
            gameManager = GameManager.instance;
            equipementManager = gameManager.equipementManager;
        }

        if (equipedItem)
        {
            equipementManager.UnEquip((int)equipedItem.itemType, isThrowItem);
        }

        icon.sprite = defaultSprite;
        slider.value = 1;
        txtCost.text = "####";
        txtDescription.text = "Item not Set";
        buttonRemove.interactable = false;
        buttonUpgrade.interactable = false;
        txtUpgrade.text = "Upgrade";
        equipedItem = null;

        if (gameManager) gameManager.ButtonSound();
    }

    public void UpgradeButton()
    {
        // Msg.instance.coin -= (int)upgradeCost;  Reduce coin

        if (txtUpgrade.text == "Heal")
        {
            equipedItem.currHealth = equipedItem.mxHealth;
            EquipItem(equipedItem);
        }
        else
        {
            equipedItem.Upgrade();
            EquipItem(equipedItem);
        }

        if (gameManager) gameManager.ButtonSound();
    }
}
