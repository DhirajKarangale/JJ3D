using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Data Object/Food Data")]
public class FoodItemData : ItemData
{
    internal override void PerformAction(Player playerStat, Item item = null)
    {
        playerStat.Eat(item);
    }
}