using UnityEngine;
using System.Collections;

public class Sling : MonoBehaviour
{
    public float forceCoef = 500;
    public GameObject SlingDirectionArrow;
    
    private GameObject slingArrow;
    private Vector2 startPos;
    private float maxMagnitude;
    private float arrowOffset;
    // Use this for initialization
    void Start ()
    {
        var spriteRenderer = SlingDirectionArrow.renderer as SpriteRenderer;
        var sprite = spriteRenderer.sprite;
        maxMagnitude = sprite.bounds.size.x;

        var collider = collider2D as CircleCollider2D;
        arrowOffset = collider.radius + 0.2f;
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }

    void OnMouseDown ()
    {
        if (enabled) {
            StartDrag (transform.position);
        }


    }
    
    void OnMouseUp ()
    {
        if (enabled) {
            EndDrag (Camera.main.ScreenToWorldPoint (Input.mousePosition));
        }
    }
    
    void OnMouseDrag ()
    {
        if (enabled) {
            UpdateArrow (Camera.main.ScreenToWorldPoint (Input.mousePosition));
        }
    }

    void StartDrag (Vector2 startPos)
    {
        this.startPos = startPos;
    }
    
    
    void EndDrag (Vector2 endPos)
    {
        DoSling (endPos);
        Object.Destroy (slingArrow);
        slingArrow = null;
    }

    void UpdateArrow (Vector2 endPos) {
        Vector2 slingVector = (endPos - startPos) * -1;
        if (slingVector.sqrMagnitude < 0.1) {
            return;
        }

        Vector2 arrowDirection = slingVector;
        arrowDirection.Normalize ();

        // Pos offset on the collider boundaries
        Vector3 arrowPosOffset = arrowDirection * arrowOffset;
        Vector3 pos = transform.position + arrowPosOffset;

        // orient
        float arrowAngle = 0;

        arrowAngle = Vector2.Angle (Vector2.right, arrowDirection);
        if (arrowDirection.y < 0) {
            arrowAngle = -arrowAngle;
        }

        Quaternion orient = Quaternion.Euler (new Vector3(0, 0, arrowAngle));

        // Assume arrow is already at max size. so always scale down.
        float arrowMagnitude = System.Math.Min (maxMagnitude, slingVector.magnitude);
        float scaleFactor = arrowMagnitude / maxMagnitude;
        Vector3 scale = Vector3.one * scaleFactor;

        if (!slingArrow) {
            slingArrow = Instantiate (SlingDirectionArrow, pos, orient) as GameObject;
        } else {
            slingArrow.transform.position = pos;
            slingArrow.transform.rotation = orient;
        }
        slingArrow.transform.localScale = scale;

        // Utils.Log ("arrowAngle:", arrowAngle, "startPos", startPos, "endPos", endPos, "slingVector", slingVector);
    }
    

    
    void DoSling (Vector2 endPos)
    {
        Vector2 slingVector = (endPos - startPos) * -1;
        Vector2 direction = slingVector;
        direction.Normalize ();

        float magnitude = System.Math.Min(slingVector.magnitude, maxMagnitude);
        float forceFactor = forceCoef * magnitude;
        
        Vector2 force = direction * forceFactor;

        Utils.Log ("Sling direction:", direction, "magnitude:", magnitude, "forceCoef:", forceCoef, "forceFactor:", forceFactor, "Force:", force);
        rigidbody2D.AddForce (force);
    }
}
