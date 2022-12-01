using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal int mxStackSize = 1;
    [SerializeField] internal string actionName;
    [SerializeField] internal float mxHealth;
    [SerializeField] internal bool isStackable;

    internal float currHealth;
    internal int id => GetInstanceID();

    internal virtual void PerformAction(PlayerStat playerStat)
    {

    }
}
