using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Flags]
public enum ModifierActionType {
    Melee = 1 << 0,
    Ranged = 1 << 1,
    Selection = 1 << 2
}

public enum ModifierContextType {
    Attack,
    Defense
}

public class AttackModifierEffect : EffectDescriptor {
    [EnumFlagsAttribute]
    public ModifierActionType modifierActionType;
    public List<Trait> srcTraits;
    public ModifierContextType contextType = ModifierContextType.Attack;

    public bool IsApplicable (PitchCharacter src, PitchCharacter target, BaseAction context) {
        if (src != target && contextType != ModifierContextType.Attack) {
            return false;
        }

        // if src is alsao target, src is defending.
        if (src == target && contextType != ModifierContextType.Defense) {
            return false;
        }

        if (context) {
            bool isAttackTypeApplicable = modifierActionType.IsFlagSet(AbilityTypeToAttackType(context.ability.abilityType));
            if (!isAttackTypeApplicable) {
                return false;
            }
        }

        bool srcTraitTrigger = false;
        if (srcTraits.Count == 0) {
            srcTraitTrigger = true;
        } 
        else {
            srcTraitTrigger = TraitUtils.HasAnyTrait(srcTraits, src.traits);
        }

        return srcTraitTrigger;
    }

    public ModifierActionType AbilityTypeToAttackType (AbilityType at) {
        ModifierActionType attackType = 0;
        switch (at) {
        case AbilityType.Ranged:
            attackType = ModifierActionType.Ranged; 
            break;
        case AbilityType.Melee:
            attackType = ModifierActionType.Melee; 
            break;
        case AbilityType.Selection:
            attackType = ModifierActionType.Selection; 
            break;
        }

        return attackType;
    }
}
