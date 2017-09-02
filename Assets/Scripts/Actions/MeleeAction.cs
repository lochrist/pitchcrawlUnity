using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MeleeAction : BaseAction, ICollisionHandler {

    public MeleeAbility meleeAbility;
    public SlingHelper helper;
    public SlingAttr slingAttr = new SlingAttr();


    public List<Collision2D> collisionList = new List<Collision2D>();

    public MeleeAction() {
    }

    public override void Init (AbilityDescriptor ability) {
        base.Init (ability);
        this.meleeAbility = BindAbility<MeleeAbility>(ability);
    }

    public override bool OnClickDown (Vector3 mouseWorldPos)
    {
        if (!helper) {
            helper = SlingHelper.Attach (actor.gameObject, slingAttr);
        }
        var acceptEvent = true;
        actionStarted = helper.StartDrag (actor.gameObject, mouseWorldPos);
        return acceptEvent;
    }
    
    public override void OnClickUp (Vector3 mouseWorldPos)
    {
        if (actionStarted) {
            helper.EndDrag (mouseWorldPos);
        }

    }

    public override void OnClickDrag (Vector3 mouseWorldPos)
    {
        if (actionStarted) {
            helper.Drag (mouseWorldPos);
        }
    }

    public override void OnWorldStabilize() {
        EndAction (!helper.slingTriggered);
    }

    public override void EndAction (bool cancelled) {
        if (helper) {
            helper.Destroy();
            helper = null;
        } 
        base.EndAction (cancelled);
        collisionList.Clear ();
        actionStarted = false;
    }


    /// ///////////////////////////////////////

    public void HandleCollisionEnter2D (Collision2D collision) {
        meleeAbility.HandleCollision (gameObject, collision, collisionList, this);
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
