using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] EquipmentSlot helmet;
    [SerializeField] EquipmentSlot vest;
    [SerializeField] EquipmentSlot weapon;
    [SerializeField] EquipmentSlot shoes;

    private GameManager gameManager;
    private PickUpSystem pickUpSystem;

    private void Start()
    {
        gameManager = GameManager.instance;
        pickUpSystem = gameManager.pickUpSystem;

        helmet.Reset();
        vest.Reset();
        weapon.Reset();
        shoes.Reset();

        weapon.OnRemove += OnWeaponRemove;
    }

    private void OnWeaponRemove()
    {
        weapon.item.ThrowItem(gameManager.playerPos.position + (gameManager.playerPos.forward * 3));
        weapon.Reset();
    }

    public void SetDefence(ItemData itemData)
    {

    }

    public void SetWeapon(Item item)
    {
        if (weapon.item) pickUpSystem.PickUp(item);
        weapon.SetData(item);
    }
}
