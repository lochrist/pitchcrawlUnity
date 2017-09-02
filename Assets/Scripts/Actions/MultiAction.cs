using UnityEngine;
using System.Collections;

public class MultiAction : BaseAction {
    public MultiAbility multiAbility;
	

    public override void Init (AbilityDescriptor ability) {
        base.Init (ability);
        this.multiAbility = BindAbility<MultiAbility>(ability);
    }

    public override void Reset () {
    }
    
    public override bool OnClickDown(Vector3 mouseWorldPos) {
        return true;
    }    
    public override void OnClickDrag(Vector3 mouseWorldPos) {
    }    
    public override void OnClickUp(Vector3 mouseWorldPos){
    }
    public override void OnWorldStabilize() {
    }
    public override void EndAction (bool cancelled = false) {
        // actor.OnEndAction (cancelled);
    }
}
