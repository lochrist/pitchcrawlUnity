using UnityEngine;
using System.Collections;

[System.Flags]
public enum CharacterStatus {
    Burn = 1 << 0,
    Freeze = 1 << 1,
    Shock = 1 << 2,
    
    Ground = 1 << 3,
    Stun = 1 << 4,
    Paralyze = 1 << 5,
    
    Slow = 1 << 6,
    Haste = 1 << 7,
    Bleeding = 1 << 8
}

public class StatusEffect : EffectDescriptor {
    [EnumFlagsAttribute]
    public CharacterStatus characterStatus;
    public ModifierType modifierType;

    protected override void DoEffectImp (GameObject src, GameObject target, BaseAction context) {
    }
    
    protected override void UndoEffectImp (GameObject target) {
    }
}
