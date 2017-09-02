using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlaceItemAttr {
    public float limitRadius;
    public float arrowZoneOfInterestRadius = 1.07f;

    public bool HasLimit () {
        return limitRadius > 0.0f;
    }
}

public class PlaceItemHelper : MonoBehaviour {

    public GameObject limitObject;

    PlaceItemAttr attr;

    public float itemRadius;
    public float maxDistance;
    public GameObject placeItemArrow;
    public Sprite placeItemArrowSprite;
    public Vector3 originalItemPosition;
    public bool isDragging;
    public bool isCollision;

    public static PlaceItemHelper Attach (GameObject owner, Vector3 placementPos, PlaceItemAttr attr) {
        var helper = owner.AddComponent<PlaceItemHelper> ();
        helper.Init(placementPos, attr);
        return helper;
    }
    
    void Init(Vector3 placementPos, PlaceItemAttr attr) {

        var placeItemArrowPrefab = Resources.Load ("Markers/PlaceItemArrow");
        placeItemArrow = Instantiate (placeItemArrowPrefab) as GameObject;

        this.attr = attr;

        // Place to object to be moved inside the limit:
        itemRadius = Utils.GetSpriteWorldSize (gameObject).x / 2;

        float itemX = placementPos.x;
        if (attr.HasLimit()) {

            var limitObjectPrefab = Resources.Load ("Markers/PlaceItemArea");
            limitObject = Instantiate (limitObjectPrefab) as GameObject;

            // Scale the limitObject accordint to the limitRadius and place it
            float spriteRadius = Utils.GetSpriteWorldSize (limitObject).x / 2;
            float newSpriteScale = attr.limitRadius / spriteRadius;
            limitObject.transform.localScale = new Vector3 (newSpriteScale, newSpriteScale, 1.0f);
            limitObject.transform.position = placementPos;

            maxDistance = attr.limitRadius - itemRadius - 0.05f;

            itemX = placementPos.x - attr.limitRadius + itemRadius + 0.1f;
        }

        originalItemPosition = new Vector3 (itemX, placementPos.y, 0.0f);
        transform.position = originalItemPosition;

        // Place the Arrow center on the to be moved object
        var arrowScale = itemRadius / attr.arrowZoneOfInterestRadius;
        placeItemArrow.transform.position = originalItemPosition;
        placeItemArrow.transform.localScale = new Vector3 (arrowScale, arrowScale, 1.0f);
    }
    
    public bool StartDrag (Vector3 startPos)
    {
        if (!collider2D.OverlapPoint (startPos))
            return false;

        collider2D.enabled = false;
        isDragging = true;
        isCollision = false;

        return true;
    }

    // returns true if flick started.
    public bool Drag (Vector3 endPos)
    {
        if (isDragging)
        {
            if (attr.HasLimit()) {
                bool isWithinLimits = Utils.IsCircleInLimits(limitObject.transform.position, attr.limitRadius, endPos, itemRadius);
                if (!isWithinLimits) {
                    Vector3 positionDirection = endPos - limitObject.transform.position;
                    positionDirection.Normalize();
                    
                    Vector3 clampedPos = limitObject.transform.position + (positionDirection * maxDistance);
                    endPos = clampedPos;
                }
            }

            transform.position = endPos;
            placeItemArrow.transform.position = endPos;

            CheckCollision(endPos);
        }
        return isDragging;
    }
    
    // returns true if flick started.
    public bool EndDrag (Vector3 endPos)
    {
        CheckCollision(endPos);
        if (isCollision) {
            transform.position = originalItemPosition;
            placeItemArrow.transform.position = originalItemPosition;
            isCollision = false;
            UpdateCollisionColor ();
        }
        collider2D.enabled = true;
        return isCollision;
    }

    public void Destroy() {
        if (placeItemArrow) {
            Object.Destroy (placeItemArrow);
            placeItemArrow = null;
        }

        if (limitObject) {
            Object.Destroy (limitObject);
            limitObject = null;
        }

        Object.Destroy (this);
    }
    
    public void OnGUI () {

    }

    void CheckCollision (Vector3 endPos) {
        isCollision = Utils.IsAnySolidBodyUnderCircle(endPos, itemRadius);
        UpdateCollisionColor ();
    }

    void UpdateCollisionColor () {
        var renderer = placeItemArrow.renderer as SpriteRenderer;
        if (isCollision) {
            renderer.color = Color.red;
        } else {
            renderer.color = Color.white;
        }
    }
    

}
