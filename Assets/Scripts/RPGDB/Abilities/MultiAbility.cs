using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ChainingDescriptor {
    public ChainingType chainingType;
    public int typeValue;// use for index
    public ChainingCondition condition;
    public int conditionValue;
}

[System.Serializable]
public class ChainedAbilityItem {
    public ChainingDescriptor chaining;
    public AbilityDescriptor ability;
}

public class MultiAbility : AbilityDescriptor {
    public List<ChainedAbilityItem> abilities;
}
