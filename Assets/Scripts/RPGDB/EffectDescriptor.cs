using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectScopeType {
    Target,
    Source,
    AreaOfEffectAroundTarget,
    AreaOfEffectAroundSource
}

public enum ModifierType {
    Add,
    Remove,
    SetTo
}

public enum EffectType {
    Damage,
    DamageModifier,
    DamageSensitivity,
    ArmorModifier,
    AttributeModifier,
    Status
}

public enum DurationType {
    Instant,
    Permanent,
    UntilXUsage,
    UntilXTurn,
    UntilXDamage,
    UntilXAttack
}


public class EffectDescriptor : ScriptableObject, System.ICloneable {
    [HideInInspector]
    public EffectType effectType;

    [EnumFlagsAttribute]
    public TargetType validTarget;
    public EffectScopeType applyEffectOn;
    public float aoeRadius;

    public DurationType durationType;
    public int maxDuration;
    public int duration;

    public GameObject specialFXPrefab;
    public Vector3 specialFXOffset = Vector3.zero;
    public Vector3 specialFXRotation = new Vector3 (70, 0, 0);

    public object Clone () {
        Object clone = Object.Instantiate(this) as Object;
        clone.name = name;
        return clone;
    }

    public void DoEffect (GameObject src, List<GameObject> targets, BaseAction context) {
        for (int i = 0; i < targets.Count; ++i) {
            DoEffect (src, targets[i], context);
        }
    }

    public void DoEffect (GameObject src, GameObject target, BaseAction context) {
        switch (applyEffectOn) {
        case EffectScopeType.Target:
            if (TargetUtils.IsTargetLegal (src, target, validTarget)) {
                ExecuteEffect (src, target, context);
            }
            break;
        case EffectScopeType.Source:
            if (TargetUtils.IsTargetLegal (src, target, validTarget)) {
                ExecuteEffect (src, src, context);
            }
            break;
        case EffectScopeType.AreaOfEffectAroundTarget:
        case EffectScopeType.AreaOfEffectAroundSource:
            var point = applyEffectOn == EffectScopeType.AreaOfEffectAroundTarget ? target.transform.position : src.transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll (point, aoeRadius);
            for (int i = 0; i < colliders.Length; ++i) {
                Collider2D collider = colliders [i];
                if (TargetUtils.IsTargetLegal (src, collider.gameObject, validTarget)) {
                    ExecuteEffect (src, collider.gameObject, context);
                }
            }
            break;
        }
    }

    public void UndoEffect (GameObject target) {
        UndoEffectImp (target);

        if (durationType != DurationType.Instant) {
            PitchCharacter character = target.GetComponent<PitchCharacter> ();
            if (character) {
                character.RemoveEffect(this);
            }
        }
    }

    //////////////////////////////////////////////////

    protected virtual void DoEffectImp(GameObject src, GameObject target, BaseAction context) {

    }

    protected virtual void UndoEffectImp(GameObject target) {
        
    }

    //////////////////////////////////////////////////

    void ExecuteEffect (GameObject src, GameObject target, BaseAction context) {
        DoEffectImp (src, target, context);
        SpawnSpecialFX (target);

        if (durationType != DurationType.Instant) {
            PitchCharacter character = target.GetComponent<PitchCharacter> ();
            if (character) {
                character.AddEffect(this);
            }
        }
    }

    void SpawnSpecialFX(GameObject target) {
        if (specialFXPrefab) {
            GameObject specialFX = Instantiate (specialFXPrefab) as GameObject;
            specialFX.transform.parent = target.transform;
            specialFX.transform.localPosition = specialFXOffset;
            specialFX.transform.rotation = Quaternion.Euler(specialFXRotation);
        }
    }
}