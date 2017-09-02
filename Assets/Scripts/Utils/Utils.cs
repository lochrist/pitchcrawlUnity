using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utils {
    static Collider2D[] collidersWithPoint = new Collider2D[5];

    static public void Log(params object[] objs) {
        var str = new System.Text.StringBuilder ();
        for (int i = 0; i < objs.Length; ++i) {
            str.Append(objs[i]);
            if (i < objs.Length - 1) {
                str.Append (" ");
            }
        }
        Debug.Log (str.ToString ());
    }

    static public Collider2D FindColliderUnderPoint(Vector3 worldPoint) {
        Collider2D collider = Physics2D.OverlapPoint (worldPoint);
        return collider;
    }
    
    static public Collider2D[] FindCollidersUnderPoint (Vector3 worldPoint) {
        Collider2D[] colliders = Physics2D.OverlapPointAll (worldPoint);
        return colliders;
    }

    static public TO ComponentToComponent<TO> (Component from) where TO : UnityEngine.Component {
        TO result = null;
        if (from != null && from.gameObject) {
            result = from.gameObject.GetComponent<TO> ();
        }
        return result;
    }

    static public List<TO> ComponentsToComponentList<FROM, TO> (FROM[] fromArray) where TO : UnityEngine.Component where FROM : UnityEngine.Component {
        List<TO> list = new List<TO> ();
        foreach(FROM collider in fromArray) {
            TO refTo = ComponentToComponent<TO>(collider);
            if (refTo) {
                list.Add (refTo);
            }
        }
        return list;
    }
	
    static public T FindObjectUnderPoint<T>(Vector3 worldPoint) where T : UnityEngine.Component {
        // Collider2D collider = FindColliderUnderPoint(worldPoint);
        T refToT = null;
        int nbCollider = Physics2D.OverlapPointNonAlloc (worldPoint, collidersWithPoint);
        for (int i = 0; i < nbCollider; ++i) {
            refToT = ComponentToComponent<T>(collidersWithPoint[i]);
            if (refToT) {
                return refToT;
            }
        }    
        return refToT;
    }

    static public List<T> FindObjectsUnderPoint<T>(Vector3 worldPoint) where T : UnityEngine.Component {
        Collider2D[] colliders = FindCollidersUnderPoint(worldPoint);
        return ComponentsToComponentList<Collider2D, T>(colliders);
    }

    static public T FindObjectUnderCircle<T>(Vector3 worldPoint, float radius) where T : UnityEngine.Component {
        Collider2D collider = Physics2D.OverlapCircle(worldPoint, radius);
        return ComponentToComponent<T>(collider);
    }

    static public List<T> FindObjectsUnderCircle<T>(Vector3 worldPoint, float radius) where T : UnityEngine.Component {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPoint, radius);
        return ComponentsToComponentList<Collider2D, T>(colliders);
    }

    static public bool IsAnyCollidersSolid(Collider2D[] colliders) {
        foreach (Collider2D c in colliders) {
            if (!c.isTrigger) {
                return true;
            }
        }
        return false;
    }

    static public bool IsAnySolidBodyUnderCircle(Vector3 worldPoint, float radius) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPoint, radius);
        return IsAnyCollidersSolid (colliders);
    }

    static public Sprite GetSpriteFromObject(GameObject obj) {
        Sprite sprite = null;
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer> ();
        if (renderer) {
            sprite = renderer.sprite;
        }
        return sprite;
    }

    static public Vector2 GetSpriteWorldSize (GameObject obj) {
        Vector2 size = new Vector2 ();
        Sprite s = GetSpriteFromObject (obj);
        if (s != null) {
            //s.bounds.extents.
            size.x = s.bounds.size.x * obj.transform.localScale.x;
            size.y = s.bounds.size.y * obj.transform.localScale.y;
        }
        return size;

    }

    static public Vector2 MouseToGUICoord(Vector3 mousePosition) {
        return new Vector2(mousePosition.x, Screen.height - mousePosition.y);
    }

    static public Vector2 WorldToGUICoord(Vector3 world) {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint (world);
        return MouseToGUICoord (screenPoint);
    }

    static public Rect WorldToGUIRect(Rect rect) {
        var p1 = new Vector2 (rect.xMin, rect.yMin);
        var p2 = new Vector2 (rect.xMax, rect.yMax);

        var p1Gui = WorldToGUICoord (p1);
        var p2Gui = WorldToGUICoord (p2);

        // Gui y axis is reverse from world
        return new Rect (p1Gui.x, p2Gui.y, p2Gui.x - p1Gui.x, p1Gui.y - p2Gui.y);
    }

    static public Vector3 MouseToWorldCoord(Vector3 mousePosition) {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint (mousePosition);
        mouseWorld.z = 0.0f;
        return mouseWorld;
    }

    static public bool IsCircleInLimits (Vector2 pos1, float radius1, Vector2 pos2, float radius2) {
        float distance = (pos1 - pos2).magnitude;
        if (distance > (radius1 + radius2)) {
            // No overlap
            return false;
        }
        else if (distance <= System.Math.Abs(radius1 - radius2)) {
            // Inside
            return true;
        }
        else
        {
            // Overlap;
            return false;
        } 
    }

    static public int Clamp(int value, int minValue, int maxValue) {
        if (value < minValue) {
            return minValue;
        } else if (value > maxValue) {
            return maxValue;
        } else {
            return value;
        }
    }

}
