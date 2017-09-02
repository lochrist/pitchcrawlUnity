using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AbilityType {
    Melee,
    Ranged,
    Move,
    Aura,
    Summon,
    Selection,
    Multi,
    Reference,
    Custom
}

public enum AbilityUsageType {
    Action,
    
    OnTargetEquips,

    OnSrcReceivesDamageFromTarget,
    OnSrcInflictsDamageToTarget,

    OnSrcKillsEnemyTarget,
    OnSrcDiesByTarget,

    OnSrcContactsTarget,

    OnSrcAttacksTarget,
    OnSrcAttackedByTarget
}

[System.Flags]
public enum ChainingCondition {
    Always = 1 << 0,
    OnDamage = 1 << 1,
    OnDeath = 1 << 2,
    OnContact = 1 << 3,
    OnAttack = 1 << 4,
    OnAttacked = 1 << 5,
    OnKill = 1 << 6
}

public enum ChainingType {
    NoChaining,
    SelfChaining,
    NextAbility,
    IndexedAbility,
}

[System.Flags]
public enum TargetType {	
    Enemy = 1 << 0,
    Ally = 1 << 1,
    Hero = 1 << 2,
    Monster = 1 << 3,
    Self = 1 << 4,
    Prop = 1 << 5,
    Surface = 1 << 6
}

public static class TargetUtils {
    public static bool IsTargetLegal (GameObject src, GameObject possibleTarget, TargetType targetType) {
        foreach (TargetType t in targetType.GetFlags()) {
            if (TargetUtils.IsTargetLegalSingle(src, possibleTarget, t)) {
                return true;
            }
        }
        return false;
    }

    static bool IsTargetLegalSingle(GameObject src, GameObject possibleTarget, TargetType targetType) {
        bool isLegal = false;
        switch (targetType) {
        case TargetType.Hero: {
            isLegal = possibleTarget.GetComponent<Hero>() != null;
            break;
        }
        case TargetType.Monster: {
            isLegal = possibleTarget.GetComponent<Monster>() != null;
            break;
        }
        case TargetType.Ally: {
            PitchCharacter targetCharacter = possibleTarget.GetComponent<PitchCharacter>();
            if (targetCharacter) {
                // Assume src is also a character
                PitchCharacter srcCharacter = src.GetComponent<PitchCharacter>();
                isLegal = srcCharacter.isHero == targetCharacter.isHero;
            }
            break;
        }
        case TargetType.Enemy: {
            PitchCharacter targetCharacter = possibleTarget.GetComponent<PitchCharacter>();
            if (targetCharacter) {
                // Assume src is also a character
                PitchCharacter srcCharacter = src.GetComponent<PitchCharacter>();
                isLegal = srcCharacter.isHero != targetCharacter.isHero;
            }
            break;
        }
        case TargetType.Self:
            isLegal = src == possibleTarget;
            break;
        case TargetType.Prop:
            isLegal = possibleTarget.GetComponent<Prop>() != null;
            break;
        case TargetType.Surface:
            isLegal = possibleTarget.GetComponent<Surface>() != null;
            break;
        }
        return isLegal;
    }
}


public enum CollisionTriggerType {
    FirstContactTarget,
    EachContactTarget
}

public class AbilityDescriptor : ScriptableObject, System.ICloneable {
    [HideInInspector]
    public AbilityType abilityType;

    // [HideInInspector]
    public AbilityContainer container;

    public Texture2D _icon;
    public AbilityUsageType usageType; // When can this ability be used, or trigger.
    public int nbActivation = 1;
    public List<EffectDescriptor> effects;

    public void BindToContainer(AbilityContainer container) {
        this.container = container;
    }

    public Texture2D Icon {
        get {
            if (_icon) {
                return _icon;
            } else {
                return container.icon;
            }
        }
    }

    public string Name { 
        get {
            return container.name;
       }
    }

    public virtual void ActivateAbility(GameObject src, GameObject target, BaseAction context) {
        for (int i = 0; i < effects.Count; ++i) {
            EffectDescriptor effect = effects[i];
            effect.DoEffect(src, target, context);
        }
    }

    public void ActivateAbility(GameObject src, List<GameObject> targets, BaseAction context) {
        for (int i = 0; i < targets.Count; ++i) {
            ActivateAbility(src, targets[i], context);
        }
    }          

    public virtual void DeactivateAbility (GameObject src) {
        for (int i = 0; i < effects.Count; ++i) {
            EffectDescriptor effect = effects[i];
            effect.UndoEffect(src);
        }
    }

    public object Clone () {
        AbilityDescriptor clone = Instantiate (this) as AbilityDescriptor;
        clone.name = name;
        for (int i = 0; i < clone.effects.Count; ++i) {
            var clonedEffect = clone.effects[i].Clone () as EffectDescriptor;
            clone.effects[i] = clonedEffect;
        }
        return clone;
    }
}