using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Data Object/Equipment Data")]
public class EquipmentItemData : ItemData
{
    internal override void PerformAction(Player playerStat, ItemData itemData = null)
    {
        playerStat.EquipDefence(itemData);
    }
}