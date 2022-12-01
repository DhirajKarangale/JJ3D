using UnityEngine;

public class WeaponItemData : ItemData
{
    [SerializeField] Category.WeaponType weaponType;
    [SerializeField] float modifier;

    internal override void PerformAction(PlayerStat playerStat, Item item)
    {
        playerStat.EquipWeapon(item, weaponType);
    }
}
