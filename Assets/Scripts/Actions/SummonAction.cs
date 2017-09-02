using UnityEngine;
using System.Collections;

public class SummonAction : BaseAction {

    public SummonAbility summonAbility;
    public PlaceItemAttr attr = new PlaceItemAttr();
    GameObject summonedObject;
    PlaceItemHelper helper;
    bool isDragging = false;

    public SummonAction () {
    }

    public override void Init (AbilityDescriptor ability) {
        base.Init (ability);
        this.summonAbility = BindAbility<SummonAbility> (ability);
        if (!this.summonAbility.summonPrefab) {
            throw new UnityException("Summon needs a prefab for summoned object (abilitydescriptor.gameObject)");
        }
    }

    public override void Reset () {
        // Starts the projectile centered on the character:
        summonedObject = GameObject.Instantiate(summonAbility.summonPrefab, actor.transform.position, Quaternion.identity) as GameObject;
        var rockSize = Utils.GetSpriteWorldSize (summonedObject);
        var rockInitialPosition = new Vector3 (actor.transform.position.x - rockSize.x - 0.05f, actor.transform.position.y, 0.0f);
        helper = PlaceItemHelper.Attach(summonedObject, rockInitialPosition, attr);

        GameManager.Instance.characterBar.ShowOkButton(this.OnOk);
    }
    
    public override bool OnClickDown (Vector3 mouseWorldPos)
    {
        var acceptEvent = true;
        isDragging = helper.StartDrag (mouseWorldPos);
        actionStarted = isDragging;
        return acceptEvent;
    }
    
    public override void OnClickUp (Vector3 mouseWorldPos)
    {
        if (isDragging) {
            helper.EndDrag(mouseWorldPos);
        }
    }
    
    public override void OnClickDrag (Vector3 mouseWorldPos)
    {
        if (isDragging) {
            helper.Drag (mouseWorldPos);
        }
    }
    
    public override void OnWorldStabilize() {
    }
    
    public override void EndAction (bool cancelled) {
        if (helper) {
            helper.Destroy ();
            helper = null;
        }

        if (cancelled) {
            GameObject.Destroy(summonedObject);
        }

        GameManager.Instance.characterBar.SetMessage("");
        GameManager.Instance.characterBar.HideOkButton(this.OnOk);

        base.EndAction (cancelled);
    }

    /////////////////////////////////////
    void Update() {
        string msg;
        if (helper.isCollision) {
            msg = "Summoning placement not legal.";
        } else {
            msg = "Press Ok when ready.";
        }

        GameManager.Instance.characterBar.SetEnableOkButton(!helper.isCollision);
        GameManager.Instance.characterBar.SetMessage(msg);
    }

    void OnOk(dfControl control, dfMouseEventArgs mouseEvent) {
        EndAction(false);
    }

    /////////////////////////////////////
    /// 
    /// 
}
