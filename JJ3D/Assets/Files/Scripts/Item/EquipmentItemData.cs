using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Data Object/Equipment Data")]
public class EquipmentItemData : ItemData
{
    internal override void PerformAction(Player playerStat, Item item = null)
    {
        playerStat.EquipDefence(item);
    }
}