using UnityEngine;

[CreateAssetMenu]
public class FoodItemData : ItemData
{
    internal override void PerformAction(Player playerStat, Item item = null)
    {
        playerStat.Eat(item);
    }
}