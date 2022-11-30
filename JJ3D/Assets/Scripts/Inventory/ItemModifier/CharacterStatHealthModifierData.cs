using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierData : CharacterStatModifierData
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerHealth playerHealth = character.GetComponent<PlayerHealth>();
        if (playerHealth)
        {
            playerHealth.IncreaseHealth(val);
        }
    }
}
