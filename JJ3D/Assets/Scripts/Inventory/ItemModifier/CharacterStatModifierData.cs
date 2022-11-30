using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStatModifierData : ScriptableObject
{
    public abstract void AffectCharacter(GameObject character, float val);
}
