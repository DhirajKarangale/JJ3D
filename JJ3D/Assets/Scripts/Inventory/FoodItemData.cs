using UnityEngine;

[CreateAssetMenu]
public class FoodItemData : ItemData
{
    [SerializeField] float modifier;

    internal override void PerformAction(PlayerStat playerStat, Item item = null)
    {
        playerStat.ChangeHealth(-modifier);
    }
}