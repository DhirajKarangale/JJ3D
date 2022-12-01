using UnityEngine;

public class WeaponItemData : ItemData
{
    [SerializeField] Category.WeaponType weaponType;
    [SerializeField] float modifier;

    internal override void PerformAction(PlayerStat playerStat)
    {
        playerStat.EquipWeapon(this, weaponType);
    }
}
