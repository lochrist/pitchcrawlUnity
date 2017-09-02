using UnityEngine;
using System.Collections;

public class Flick : MonoBehaviour
{

    public float minDeltaTime = 200;
    public float maxDeltaTime = 1000;
    public float forcePool = 400000;

    
    private Vector2 startPos;
    private float startTime;
    private static int touchCount = 0;

    void Start ()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        
    }
    
    void FixedUpdate ()
    {
        if (Input.GetButtonDown ("Fire1")) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            Collider2D coll = Physics2D.OverlapPoint (mousePos);
            if (coll == collider2D) {
                ++touchCount;
                // Debug.Log ("Touch " + touchCount + " " + collider2D.gameObject);
            }
        }

    }
    
    void OnMouseDown ()
    {
        StartDrag (Input.mousePosition);
    }
    
    void OnMouseUp ()
    {
        EndDrag (Input.mousePosition);
    }
    
    void OnMouseDrag ()
    {
        Drag (Input.mousePosition);
    }
    
    void StartDrag (Vector2 startPos)
    {
        this.startPos = startPos;
        this.startTime = Time.time;
    }
    
    void Drag (Vector2 endPos)
    {
        Vector2 direction = endPos - startPos;
        // Debug.Log("endPos " + endPos + " startPos " + mStartPos + " direction " + direction + " magnitude " + direction.sqrMagnitude);
        if (direction.sqrMagnitude > 10000) {
            float endTime = Time.time;
            DoFlick(endTime, endPos);
        }
    }
    
    void EndDrag (Vector2 endPos)
    {
        float endTime = Time.time;
        DoFlick (endTime, Input.mousePosition);
    }
    
    void DoFlick (float endTime, Vector2 endPos)
    {
        if (startTime <= 0.0f) {
            return;
        }
        
        float deltaTime = (endTime - startTime) * 1000;
        deltaTime = Mathf.Clamp (deltaTime, minDeltaTime, maxDeltaTime);
        
        Vector2 direction = endPos - startPos;
        direction.Normalize ();
        
        float forceFactor = forcePool / deltaTime;
        
        Vector2 force = direction * forceFactor;
        
        // Utils.Log ("Flick direction:", direction, "deltaTime:", deltaTime, "forceFactor:", forceFactor, "Force:", force);
        rigidbody2D.AddForce (force);
        
        startTime = 0;
    }
}