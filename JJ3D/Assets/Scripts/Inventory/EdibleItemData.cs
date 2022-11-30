using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EdibleItemData : InventoryItemData, IDestroyableItem, IItemAction
{
    [SerializeField] List<ModifierData> modifiersData = new List<ModifierData>();

    public string ActionName => "Consume";

    public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
    {
        foreach (ModifierData data in modifiersData)
        {
            data.statModifier.AffectCharacter(character, data.value);
        }
        return true;
    }
}

public interface IDestroyableItem
{

}

public interface IItemAction
{
    public string ActionName { get; }

    bool PerformAction(GameObject character, List<ItemParameter> itemState);
}

[System.Serializable]
public class ModifierData
{
    public CharacterStatModifierData statModifier;
    public float value;
}