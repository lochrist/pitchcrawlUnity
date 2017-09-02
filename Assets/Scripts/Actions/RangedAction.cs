using UnityEngine;
using System.Collections;

public class RangedAction : BaseAction, ICollisionHandler {
    public RangedAbility rangedAbility;
    public SlingHelper helper;
    public SlingAttr slingAttr = new SlingAttr();

    public PlaceItemAttr placementAttr = new PlaceItemAttr();
    public PlaceItemHelper placeItemHelper;

    public GameObject projectile;

    public bool isMovingProjectile;
    public bool isProjectileReady;
    public float timeProjectileReady;
    public float placementTime = 3.0f;
    
    public RangedAction() {
        placementAttr.limitRadius = 1.0f;
    }

    public override void Init (AbilityDescriptor ability) {
        base.Init (ability);

        this.rangedAbility = BindAbility<RangedAbility>(ability);

        if (!this.rangedAbility.projectilePrefab) {
            throw new UnityException("Ranged Attack needs a prefab for projectile (abilitydescriptor.gameObject)");
        }
    }

    public override void Reset() {
        isMovingProjectile = true;
        isProjectileReady = true;
        timeProjectileReady = Time.time;
        
        // Starts the projectile centered on the character:
        projectile = GameObject.Instantiate(rangedAbility.projectilePrefab, actor.transform.position, Quaternion.identity) as GameObject;
        placeItemHelper = PlaceItemHelper.Attach(projectile, actor.transform.position, placementAttr);
    }
    
    public override bool OnClickDown (Vector3 mouseWorldPos)
    {
        var acceptEvent = true;
        if (isMovingProjectile) {
            actionStarted = placeItemHelper.StartDrag (mouseWorldPos);
            isProjectileReady = false;
        } else {
            helper = SlingHelper.Attach (projectile, slingAttr, this);
            helper.StartDrag (projectile, mouseWorldPos);
        }

        return acceptEvent;
    }
    
    public override void OnClickUp (Vector3 mouseWorldPos)
    {
        if (isMovingProjectile) {
            placeItemHelper.EndDrag(mouseWorldPos);
            isProjectileReady = true;
            timeProjectileReady = Time.time;
        } else {
            if (!helper.EndDrag (mouseWorldPos)) {
                // CancelAction ();
            } else {
                // Arrow has been slinged: mark it has a trigger (collision free object):
                projectile.collider2D.isTrigger = !rangedAbility.physicProjectile;
            }
        }
    }
    
    public override void OnClickDrag (Vector3 mouseWorldPos)
    {
        if (isMovingProjectile) {
            placeItemHelper.Drag(mouseWorldPos);
        } else {
            helper.Drag (mouseWorldPos);
        }

    }
    
    public override void OnWorldStabilize() {
        if (helper && helper.slingTriggered) {
            EndAction (!helper.slingTriggered);
        }
    }
    
    public override void EndAction (bool cancelled) {
        if (helper) {
            helper.Destroy();
            helper = null;
        }
        if (projectile && (cancelled || rangedAbility.destroyAtEndAction || !rangedAbility.physicProjectile)) {
            GameManager.Instance.DestroyBody (projectile);
            projectile = null;
        }
        DestroyPlaceItemHelper ();
        base.EndAction (cancelled);
    }

    /////////////////////////////////////

    public override void OnGUI () {    
        base.OnGUI ();
        if (isProjectileReady) {
            float delta = Time.time - timeProjectileReady;
            string msg;
            if (delta > placementTime) {
                msg = "Shoot!!";
                SwitchToFlick();
            } else {
                msg = "Shooting in: " + System.Math.Ceiling(placementTime - delta);
            }

            Rect worldRect = new Rect(transform.position.x - placementAttr.limitRadius, 
                                      transform.position.y - placementAttr.limitRadius, 
                                      2.0f * placementAttr.limitRadius, 
                                      0.4f);
            Rect guiRect = Utils.WorldToGUIRect(worldRect);
            GUI.Label(guiRect, msg);
        }
    }

    void SwitchToFlick() {
        if (isMovingProjectile && isProjectileReady) {
            isMovingProjectile = false;
            isProjectileReady = false;
            DestroyPlaceItemHelper ();
        }
    }

    void DestroyPlaceItemHelper () {
        if (placeItemHelper) {
            placeItemHelper.Destroy();
            placeItemHelper = null;
        }
    }

    /////////////////////////////////////


    public void HandleCollisionEnter2D (Collision2D collision) {
        rangedAbility.ActivateAbility(actor.gameObject, collision.gameObject, this);
        if (rangedAbility.collisionType == CollisionTriggerType.FirstContactTarget) {
            EndAction(false);
        }
    }
    public void HandleCollisionStay2D (Collision2D collision) {
    }
    public void HandleCollisionExit2D (Collision2D collision) {
    }
    
    public void HandleTriggerEnter2D (Collider2D collider) {
        rangedAbility.ActivateAbility(actor.gameObject, collider.gameObject, this);
        if (rangedAbility.collisionType == CollisionTriggerType.FirstContactTarget && !collider.isTrigger) {
            EndAction(false);
        }
    }
    public void HandleTriggerStay2D (Collider2D collider) {
    }
    public void HandleTriggerExit2D (Collider2D collider) {
    }
}
