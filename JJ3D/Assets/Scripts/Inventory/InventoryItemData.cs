using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryItemData : ScriptableObject
{
    [field: SerializeField]
    public int MxStackSize { get; set; } = 1;

    [field: SerializeField]
    public int Count { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public Sprite Sprite { get; set; }

    [field: SerializeField]
    public bool isStackable { get; set; }

    public int id => GetInstanceID();


}
