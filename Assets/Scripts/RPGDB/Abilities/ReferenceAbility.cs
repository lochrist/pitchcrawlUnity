using UnityEngine;
using System.Collections;

public enum ReferenceAbilityType {
    MeleeAttack,
    RangedAttack,
    Move,
    ByName
}

public class ReferenceAbility : AbilityDescriptor {
    public ReferenceAbilityType referenceType;
    public string referenceName;

    public AbilityDescriptor ResolveReference(PitchCharacter character) {
        AbilityDescriptor resolvedAbility = null;
        switch (referenceType) {
        case ReferenceAbilityType.MeleeAttack:
            resolvedAbility = character.meleeAttack;
            break;
        case ReferenceAbilityType.Move:
            resolvedAbility = character.move;
            break;
        case ReferenceAbilityType.RangedAttack:
            resolvedAbility = character.rangedAttack;
            break;
        case ReferenceAbilityType.ByName:
            resolvedAbility = character.FindActionAbility(referenceName);
            break;
        }
        return resolvedAbility;
    }
}
