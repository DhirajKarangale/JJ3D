using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField] EquippableItemData weapon;
    [SerializeField] InventoryData inventoryData;
    [SerializeField] List<ItemParameter> parametersToModify;
    [SerializeField] List<ItemParameter> itemCurrState;

    public void SetWeapon(EquippableItemData weaponItemData, List<ItemParameter> itemState)
    {
        if (weapon != null)
        {
            inventoryData.AddItem(weapon, 1, itemCurrState);
        }

        this.weapon = weaponItemData;
        this.itemCurrState = new List<ItemParameter>(itemState);
        ModifyParameter();
    }

    private void ModifyParameter()
    {
        foreach (var parameter in parametersToModify)
        {
            if (itemCurrState.Contains(parameter))
            {
                int index = itemCurrState.IndexOf(parameter);
                float newValue = itemCurrState[index].value + parameter.value;
                itemCurrState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue
                };
            }
        }
    }
}
