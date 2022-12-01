using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] EquipmentManager equipmentManager;

    private float mxHealth;
    private float currhealth;
    private float speed;
    private float defence;
    private float attack;

    internal void ChangeHealth(float amount)
    {
        currhealth -= amount;
    }

    internal void EquipWeapon(ItemData item, Category.WeaponType weaponType)
    {
        equipmentManager.SetWeapon(item);
    }
}
