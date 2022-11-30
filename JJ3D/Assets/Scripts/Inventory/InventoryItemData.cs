using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItemData : ScriptableObject
{
    [field: SerializeField]
    public int MxStackSize { get; set; } = 1;

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public Sprite Sprite { get; set; }

    [field: SerializeField]
    public bool isStackable { get; set; }

    public int id => GetInstanceID();

    [field: SerializeField]
    public List<ItemParameter> DefaultParameterList { get; set; }
}


[Serializable]
public struct ItemParameter : IEquatable<ItemParameter>
{
    public ItemParameterData itemParameter;
    public float value;

    public bool Equals(ItemParameter other)
    {
        return other.itemParameter == itemParameter;
    }
}
