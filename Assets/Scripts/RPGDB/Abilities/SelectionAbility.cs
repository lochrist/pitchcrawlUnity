using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionAbility : AbilityDescriptor {
    [EnumFlagsAttribute]
    public TargetType selectionTargetType = TargetType.Enemy;
    public int maxNumberOfSelection = 1;
    public bool executeWithoutConfirmation = false;

    public bool IsSingleTarget () {
        return maxNumberOfSelection == 1;
    }
}
