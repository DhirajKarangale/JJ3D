using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Data Object/Food Data")]
public class FoodItemData : ItemData
{
    internal override void PerformAction(Player playerStat, ItemData itemData = null)
    {
        playerStat.Eat(itemData);
    }
}