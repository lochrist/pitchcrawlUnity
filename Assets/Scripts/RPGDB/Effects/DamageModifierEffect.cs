using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageModifierEffect : AttackModifierEffect {
    public AttributeValue damageModifier;
    [EnumFlagsAttribute]
    public DamageType damageType;
}