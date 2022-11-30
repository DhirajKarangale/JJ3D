using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquippableItemData : InventoryItemData, IDestroyableItem, IItemAction
{
    public string ActionName => "Equip";

    public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
    {
        AgentWeapon weaponSystem = character.GetComponent<AgentWeapon>();
        if (weaponSystem)
        {
            weaponSystem.SetWeapon(this, itemState == null ? DefaultParameterList : itemState);
            return true;
        }

        return false;
    }
}
