using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal string actionName;
    [SerializeField] internal float mxHealth;
    [SerializeField] internal float modifier;
    internal float currHealth;

    internal virtual void PerformAction(Player playerStat, Item item) { }
}
