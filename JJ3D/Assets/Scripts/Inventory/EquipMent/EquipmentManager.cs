using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] EquipmentSlot helmet;
    [SerializeField] EquipmentSlot vest;
    [SerializeField] EquipmentSlot weapon;
    [SerializeField] EquipmentSlot shoes;

    private void Start()
    {
        helmet.Reset();
        vest.Reset();
        weapon.Reset();
        shoes.Reset();
    }

    public void SetDefence(ItemData itemData)
    {

    }

    public void SetWeapon(ItemData itemData)
    {
        weapon.SetData(itemData.sprite, itemData.mxHealth / itemData.currHealth);
    }
}
