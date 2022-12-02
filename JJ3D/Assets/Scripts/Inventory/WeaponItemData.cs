using UnityEngine;

public class WeaponItemData : ItemData
{
    internal override void PerformAction(Player playerStat, Item item)
    {
        playerStat.EquipWeapon(item);
    }
}
