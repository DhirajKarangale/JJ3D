using UnityEngine;

[CreateAssetMenu]
public class FoodItemData : ItemData
{
    [SerializeField] float modifier;

    internal override void PerformAction(PlayerStat playerStat)
    {
        playerStat.ChangeHealth(-modifier);
    }
}