using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data Object/Weapon Data")]
public class WeaponItemData : ItemData
{
    internal override void PerformAction(Player playerStat, ItemData itemData)
    {
        playerStat.EquipWeapon(itemData);
    }
}
