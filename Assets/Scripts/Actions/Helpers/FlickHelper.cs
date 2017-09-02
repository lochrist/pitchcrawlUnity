using UnityEngine;
using System.Collections;

[System.Serializable]
public class FlickAttr {
    public float maxForce = 1500;
    public float timeSpeed = 0.7f;
    public float maxDragLength = 2.5f;
}

public class FlickHelper : MonoBehaviour
{
    public FlickAttr flickAttr;
    public bool flickTriggered = false;

    Texture2D timeTex;
    Texture2D dragTex;
    Texture2D backgroundTex;

    Vector2 flickDirection;
    Vector2 startPos;
    float flickLength;
    float startTime;
    float timeRatio;
    float dragRatio;
    ICollisionHandler collisionHandler;

    Rect powerBarRect;
    GameObject bodyFlicked;

    public static FlickHelper Attach (GameObject owner, FlickAttr attr, ICollisionHandler handler = null) {
        var helper = owner.AddComponent<FlickHelper> ();
        helper.Init(attr, handler);
        return helper;
    }

    void Init(FlickAttr attr, ICollisionHandler handler) {
        collisionHandler = handler;
        flickAttr = attr;
        timeTex = Resources.Load<Texture2D>("RedRectangle");
        dragTex = Resources.Load<Texture2D>("GreenRectangle");
        backgroundTex = Resources.Load<Texture2D>("WhiteRectangle");
    }

    public bool StartDrag (GameObject bodyFlicked, Vector2 startPos)
    {
        if (!bodyFlicked.collider2D.OverlapPoint (startPos))
            return false;

        flickTriggered = false;
        this.bodyFlicked = bodyFlicked;
        this.startPos = startPos;
        this.startTime = Time.time;
        timeRatio = 0.0f;
        dragRatio = 0.0f;

        Reset ();

        var collider = bodyFlicked.collider2D as CircleCollider2D;
        var worldpowerBarPos = bodyFlicked.transform.position;
        worldpowerBarPos.y -= collider.radius - 0.2f;
        var bodyPos = Utils.WorldToGUICoord(worldpowerBarPos);

        powerBarRect = new Rect (bodyPos.x - 50.0f, bodyPos.y + 20.0f, 100.0f, 20.0f);

        return true;
    }

    void Reset () {
        timeRatio = 0.0f;
        dragRatio = 0.0f;
    }

    // returns true if flick started.
    public bool Drag (Vector2 endPos)
    {
        var result = false;
        UpdateFlickForce (Time.time, endPos);
        // Debug.Log("endPos " + endPos + " startPos " + mStartPos + " direction " + direction + " magnitude " + direction.sqrMagnitude);
        if (dragRatio + timeRatio >= 1.0f) {
            result = DoFlick(endPos);
        }
        return result;
    }

    // returns true if flick started.
    public bool EndDrag (Vector2 endPos)
    {
        return DoFlick (endPos);
    }

    public void OnGUI () {
        GUI.BeginGroup(powerBarRect);
        GUI.DrawTexture(new Rect(0,0, powerBarRect.width, powerBarRect.height), backgroundTex);
        
        float dragSizeX = powerBarRect.width * dragRatio;
        GUI.BeginGroup(new Rect(0, 0, dragSizeX, powerBarRect.height));
        GUI.DrawTexture(new Rect(0,0, powerBarRect.width, powerBarRect.height), dragTex);
        GUI.EndGroup();
        
        //draw the filled-in part:
        float timeX = powerBarRect.width - (powerBarRect.width * timeRatio);
        GUI.BeginGroup(new Rect(timeX,0, powerBarRect.width, powerBarRect.height));
        GUI.DrawTexture(new Rect(0,0, powerBarRect.width, powerBarRect.height), timeTex);
        GUI.EndGroup();
        
        GUI.EndGroup();
    }
    
    bool UpdateFlickForce(float endTime, Vector2 endPos) {
        if (startTime <= 0.0f) {
            return false;
        }

        flickDirection = endPos - startPos;
        flickLength = flickDirection.magnitude;

        flickDirection.Normalize ();

        var deltaTime = endTime - startTime;

        timeRatio = deltaTime * flickAttr.timeSpeed;
        timeRatio = System.Math.Min (timeRatio, 1.0f);

        flickLength = System.Math.Min (flickLength, flickAttr.maxDragLength);
        dragRatio = flickLength / flickAttr.maxDragLength;
        dragRatio = System.Math.Min (dragRatio, 1-timeRatio);

        return true;
    }


    bool DoFlick (Vector2 endPos)
    {
        float endTime = Time.time;
        if (!UpdateFlickForce (endTime, endPos)) {
            Reset ();
            return false;
        }

        if (flickLength < 1) {
            Reset ();
            return false;
        }


        flickTriggered = true;

        var flickForceCoef = dragRatio * flickAttr.maxForce;
        Vector2 force = flickDirection * flickForceCoef;
        
        // Utils.Log ("Flick direction:", flickDirection, "timeRatio:", timeRatio, "flickLength", flickLength, "dragRatio:", dragRatio, "flickForceCoef:", flickForceCoef, "force:", force);

        GameManager.Instance.AddBody (bodyFlicked);
        GameManager.Instance.trackMovingBody = true;
        GameManager.Instance.FollowBody (bodyFlicked);
        bodyFlicked.rigidbody2D.AddForce (force);
        
        startTime = 0;
        return false;
    }

    public void Destroy () {
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