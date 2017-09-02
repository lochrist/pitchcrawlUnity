using UnityEngine;
using System.Collections;

public enum DamageModifierType {
    Nothing,
    Immune,
    Resistant,
    Sensitive,
    Weakness
}

public class DamageSensitivityEffect : EffectDescriptor {
    [EnumFlagsAttribute]
    public DamageType damageType;

    [HideInInspector]
    public float damageModifier {
        get {
            float dmgMod = 1.0f;
            switch (dmgModType) {
            case DamageModifierType.Immune:
                dmgMod = 0.0f;
                break;
            case DamageModifierType.Resistant:
                dmgMod = 0.5f;
                break;
            case DamageModifierType.Sensitive:
                dmgMod = 1.5f;
                break;
            case DamageModifierType.Weakness:
                dmgMod = 2.0f;
                break;
            }
            return dmgMod;
        }
    }

    public DamageModifierType dmgModType;
}
