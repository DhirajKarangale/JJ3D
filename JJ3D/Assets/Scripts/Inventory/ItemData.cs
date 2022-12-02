using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField] internal float modifier;
    // [SerializeField] internal Category.WeaponType weaponType;
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal string actionName;
    [SerializeField] internal float mxHealth;
    internal float currHealth;

    internal virtual void PerformAction(Player playerStat, Item item) { }
}
