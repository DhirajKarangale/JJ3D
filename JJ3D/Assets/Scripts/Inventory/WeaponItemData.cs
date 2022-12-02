using UnityEngine;

public class WeaponItemData : ItemData
{
    internal override void PerformAction(PlayerStat playerStat, Item item)
    {
        playerStat.EquipWeapon(item);
    }
}
