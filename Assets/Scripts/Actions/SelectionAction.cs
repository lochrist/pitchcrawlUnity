using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionAction : BaseAction {
    public SelectionAbility selectionAbility;
    public List<GameObject> targets = new List<GameObject>();

    public override void Init (AbilityDescriptor ability) {
        base.Init (ability);
        this.selectionAbility = BindAbility<SelectionAbility>(ability);
    }

    public override void Reset () {
        if (selectionAbility.selectionTargetType.IsFlagSet(TargetType.Self)) {
            PickTarget(gameObject);
        }

        GameManager.Instance.characterBar.ShowOkButton(this.OnOk);
        GameManager.Instance.characterBar.SetMessage("Press Ok when ready");
    }

    public override bool OnClickDown (Vector3 mouseWorldPos)
    {
        var acceptEvent = true;
        Collider2D collider = Utils.FindColliderUnderPoint (mouseWorldPos);

        if (collider) {
            if (TargetUtils.IsTargetLegal(gameObject, collider.gameObject, selectionAbility.selectionTargetType)) {
                PickTarget (collider.gameObject);
            } 
        }

        return acceptEvent;
    }
    
    public override void OnClickUp (Vector3 mouseWorldPos)
    {

    }
    
    public override void OnClickDrag (Vector3 mouseWorldPos)
    {

    }

    public void OnOk(dfControl control, dfMouseEventArgs mouseEvent) {
        EndAction(false);
    }

    public override void EndAction (bool cancelled) {
        if (!cancelled && targets.Count > 0) {
            if (selectionAbility.IsSingleTarget()) {
                selectionAbility.ActivateAbility(gameObject, targets[0], this);
            } else {
                selectionAbility.ActivateAbility(gameObject, targets, this);
            }
        }

        GameManager.Instance.characterBar.SetMessage("");
        GameManager.Instance.characterBar.HideOkButton(this.OnOk);

        base.EndAction (cancelled);
        RemoveTarget ();
    }

    void Update () {
        if (selectionAbility.executeWithoutConfirmation) {
            EndAction(false);
        }
    }


    GameObject CurrentTarget () {
        GameObject result = null;
        if (targets.Count > 0) {
            result = targets[0];
        } 
        return result;
    }

    void PickTarget (GameObject obj) {
        if (selectionAbility.IsSingleTarget()) {
            // Single Selection:
            if (CurrentTarget() == obj) {
                RemoveTarget();
            } else {
                RemoveTarget();
                AddTarget (obj);
            }
        } else {
            if (targets.Contains(obj)) {
                RemoveTarget (obj);
            } else {
                AddTarget (obj);
            }
        }
    }

    void AddTarget (GameObject obj) {
        targets.Add (obj);

        SpriteRenderer renderer = obj.renderer as SpriteRenderer;
        renderer.color = Color.green;
    }

    void RemoveTarget (GameObject obj = null) {
        if (obj == null) {
            for (int i = 0; i < targets.Count; ++i) {
                SpriteRenderer renderer = targets[i].renderer as SpriteRenderer;
                renderer.color = Color.white;
            }
            targets.Clear ();
        } else {
            SpriteRenderer renderer = obj.renderer as SpriteRenderer;
            renderer.color = Color.white;
            targets.Remove(obj);
        }

    }
}
