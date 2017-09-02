using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MoveAction : BaseAction, ICollisionHandler {
    public MoveAbility moveAbility;
    public FlickHelper helper;
    public FlickAttr flickAttr = new FlickAttr();

    public List<Collision2D> collisionList = new List<Collision2D>();

    public MoveAction() {
    }

    public override void Init (AbilityDescriptor ability) {
        base.Init (ability);
        this.moveAbility = BindAbility<MoveAbility>(ability);
    }

    public override bool OnClickDown (Vector3 mouseWorldPos)
    {
        if (!helper) {
            helper = FlickHelper.Attach(actor.gameObject, flickAttr);
        }

        var acceptEvent = true;
        actionStarted = helper.StartDrag (actor.gameObject, mouseWorldPos);
        return acceptEvent;
    }
    
    public override void OnClickUp (Vector3 mouseWorldPos)
    {
        helper.EndDrag (mouseWorldPos);
    }
    
    public override void OnClickDrag (Vector3 mouseWorldPos)
    {
        helper.Drag (mouseWorldPos);
    }

    public override void OnWorldStabilize() {
        EndAction (!helper.flickTriggered);
    }

    public override void EndAction (bool cancelled) {
        if (helper) {
            helper.Destroy ();
            helper = null;
        } 
        base.EndAction (cancelled);
        collisionList.Clear ();
    }

    /// ///////////////////////////////////////
    
    public void HandleCollisionEnter2D (Collision2D collision) {
        moveAbility.HandleCollision (gameObject, collision, collisionList, this);
    }
    public void HandleCollisionStay2D (Collision2D collision) {
    }
    public void HandleCollisionExit2D (Collision2D collision) {
    }
    
    public void HandleTriggerEnter2D (Collider2D collider) {
    }
    public void HandleTriggerStay2D (Collider2D collider) {
    }
    public void HandleTriggerExit2D (Collider2D collider) {
    }
}
