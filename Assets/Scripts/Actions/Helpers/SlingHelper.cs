using UnityEngine;
using System.Collections;

[System.Serializable]
public class SlingAttr {
    public float maxForce = 1500;
}

public class SlingHelper : MonoBehaviour {

    public SlingAttr slingAttr;
    public bool slingTriggered = false;

    static GameObject slingArrow;
    static float maxMagnitude;

    Vector3 startPos;
    float arrowOffset;
    ICollisionHandler collisionHandler;

    float forceRatio = 0;
    Vector3 forceDirection;

    GameObject bodySlinged;

    static public SlingHelper Attach(GameObject owner, SlingAttr attr, ICollisionHandler handler = null) {
        var helper = owner.AddComponent<SlingHelper> ();
        helper.Init (attr, handler);
        return helper;
    }

    void Init(SlingAttr attr, ICollisionHandler handler) {
        collisionHandler = handler;
        if (!slingArrow) {
            var slingArrowResource = Resources.Load ("Markers/SlingDirectionArrow") as GameObject;
            var sprite = Utils.GetSpriteFromObject (slingArrowResource);
            maxMagnitude = sprite.bounds.size.x;
            slingArrow = Instantiate(slingArrowResource) as GameObject;
            slingArrow.SetActive(false);
        }

        slingAttr = attr;
    }

    public void Awake () {

    }

    public bool StartDrag (GameObject toSling, Vector3 startPos)
    {
        if (!toSling.collider2D.OverlapPoint (startPos))
            return false;

        slingTriggered = false;
        bodySlinged = toSling;

        var collider = bodySlinged.collider2D as CircleCollider2D;
        arrowOffset = collider.radius + 0.2f;
        this.startPos = startPos;

        return true;
    }

    public void Drag(Vector3 startPos) {
        UpdateArrow (startPos);
    }
    
    public bool EndDrag (Vector3 endPos)
    {
        UpdateArrow (startPos);
        var result = DoSling (endPos);
        slingArrow.SetActive (false);
        return result;
    }

    bool UpdateSlingForce(Vector3 endPos) {
        Vector3 slingVector = (endPos - startPos) * -1;
        if (slingVector.sqrMagnitude < 0.1) {
            return false;
        }

        forceDirection = slingVector;
        forceDirection.Normalize ();

        float arrowMagnitude = System.Math.Min (maxMagnitude, slingVector.magnitude);
        forceRatio = arrowMagnitude / maxMagnitude;

        return true;
    }

    void UpdateArrow (Vector3 endPos) {
        if (!UpdateSlingForce (endPos))
            return;
        // Pos offset on the collider boundaries
        Vector3 arrowPosOffset = forceDirection * arrowOffset;
        Vector3 arrowPos = bodySlinged.transform.position + arrowPosOffset;
        
        // Angle is between 0 and 180.
        float arrowAngleWithXAxis = Vector3.Angle (Vector3.right, forceDirection);
        if (forceDirection.y < 0) {
            arrowAngleWithXAxis = -arrowAngleWithXAxis;
        }

        // Rotation around the Z Axis. R
        Quaternion arrowRotation = Quaternion.Euler (new Vector3(0, 0, arrowAngleWithXAxis));
        Vector3 arrowScale = Vector3.one * forceRatio;
        
        if (!slingArrow.activeInHierarchy) {
            slingArrow.SetActive(true);
        }

        slingArrow.transform.position = arrowPos;
        slingArrow.transform.rotation = arrowRotation;
        slingArrow.transform.localScale = arrowScale;
        
        // Utils.Log ("arrowAngle:", arrowAngle, "startPos", startPos, "endPos", endPos, "slingVector", slingVector);
    }

    bool DoSling (Vector3 endPos)
    {
        if (!UpdateSlingForce (endPos)) {
            return false;
        }

        slingTriggered = true;
        var forceMagnitude = forceRatio * slingAttr.maxForce;
        Vector3 force = forceDirection * forceMagnitude;
        
        // Utils.Log ("Sling direction:", forceDirection, "forceMagnitude:", forceMagnitude, "Force:", force);

        GameManager.Instance.AddBody (bodySlinged);
        GameManager.Instance.trackMovingBody = true;
        GameManager.Instance.FollowBody (bodySlinged);
        bodySlinged.rigidbody2D.AddForce (force);

        return true;
    }

    public void Destroy () {
        Object.Destroy (slingArrow);
        Object.Destroy (this);
    }

    /////////////////////////////////////////////////////
    
    public virtual void OnCollisionEnter2D (Collision2D collision) {
        if (collisionHandler != null) {
            collisionHandler.HandleCollisionEnter2D(collision);
        }
    }
    public virtual void OnCollisionStay2D (Collision2D collision) {
        if (collisionHandler != null) {
            collisionHandler.HandleCollisionStay2D(collision);
        }        
    }    
    public virtual void OnCollisionExit2D (Collision2D collision) {
        if (collisionHandler != null) {
            collisionHandler.HandleCollisionExit2D(collision);
        }
    }
    
    public virtual void OnTriggerEnter2D (Collider2D collider) {
        if (collisionHandler != null) {
            collisionHandler.HandleTriggerEnter2D(collider);
        }
    }
    public virtual void OnTriggerStay2D (Collider2D collider) {
        if (collisionHandler != null) {
            collisionHandler.HandleTriggerStay2D(collider);
        }
    }
    public virtual void OnTriggerExit2D (Collider2D collider) {
        if (collisionHandler != null) {
            collisionHandler.HandleTriggerExit2D(collider);
        }
    }
}
