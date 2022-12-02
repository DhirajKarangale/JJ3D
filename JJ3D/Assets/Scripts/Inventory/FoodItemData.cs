using UnityEngine;

[CreateAssetMenu]
public class FoodItemData : ItemData
{
    internal override void PerformAction(PlayerStat playerStat, Item item = null)
    {
        playerStat.ChangeHealth(item);
    }
}