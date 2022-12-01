using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal string actionName;
    [SerializeField] internal float mxHealth;

    internal float currHealth;
    internal int id => GetInstanceID();

    internal virtual void PerformAction(PlayerStat playerStat, Item item) { }
}
