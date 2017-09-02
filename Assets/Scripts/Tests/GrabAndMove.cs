using UnityEngine;
using System.Collections;

public class GrabAndMove : MonoBehaviour {
	
    public GameObject limitCircle;
    public float limitsRadius;
    public float arrowRadius;
    public bool mIsGrabbed;
    public bool isCollision;
    public bool isWithinLimits;
    public bool isAsleep;
    Vector3 originalPosition;
	// Use this for initialization
	void Start () {

        // Assuming limits is a circle
        var sprite = Utils.GetSpriteFromObject (limitCircle);
        limitsRadius = sprite.bounds.size.x / 2 * limitCircle.gameObject.transform.localScale.x;

        sprite = Utils.GetSpriteFromObject (gameObject);
        arrowRadius = sprite.bounds.size.x / 2 * gameObject.transform.localScale.x;

        limitCircle.transform.position = gameObject.transform.position;

        rigidbody2D.Sleep ();
	}
	
	// Update is called once per frame
	void Update () {
        isAsleep = rigidbody2D.IsSleeping();
	}
	
	void OnMouseDown()
	{
		// OnUpdate is NOT called is not enabled BUT OnMouseDown will be called
		if (enabled)
		{
            originalPosition = transform.position;
			collider2D.enabled = false;
            rigidbody2D.isKinematic = true;
			mIsGrabbed = true;
            isCollision = false;

		}
	}
	
	void OnMouseDrag()
	{
		if (mIsGrabbed)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = transform.position.z;
			transform.position = mousePos;

            var movedCollider = rigidbody2D.collider2D as CircleCollider2D;
            isCollision = Utils.IsAnySolidBodyUnderCircle(mousePos, movedCollider.radius);

            isWithinLimits = Utils.IsCircleInLimits(originalPosition, limitsRadius, mousePos, arrowRadius);
		}
	}
	
	void OnMouseUp()
	{
        if (isCollision || !isWithinLimits) {
            transform.position = originalPosition;
        }

		mIsGrabbed = false;
        collider2D.enabled = true;
        rigidbody2D.isKinematic = false;
        isCollision = false;
        // rigidbody2D.isKinematic = false;
	}

    void OnGUI () {
        if (isCollision) {
            GUI.Label (new Rect (10, 10, 200, 100), "Pieces cannot be touched!");
        } else if (!isWithinLimits) {
            GUI.Label (new Rect (10, 10, 200, 100), "Arrow must stays in limits");
        }
    }
}
