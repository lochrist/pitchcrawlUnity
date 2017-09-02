using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttributeEffect : EffectDescriptor {
    public AttributeType attributeType;
    public AttributeValue modifier;

    protected override void DoEffectImp (GameObject src, GameObject target, BaseAction context) {
    }
    
    protected override void UndoEffectImp (GameObject target) {
    }
}
