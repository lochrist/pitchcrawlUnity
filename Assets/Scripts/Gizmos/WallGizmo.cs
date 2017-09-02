using UnityEngine;
using System.Collections;

public class WallGizmo : MonoBehaviour
{
    void Awake()
    {
    }   

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "wall.png", true);
        Gizmos.color = Color.cyan;

        // Don't draw line when the object is selected as the polygon collider will draw these itself.
        if (!UnityEditor.Selection.Contains(gameObject))
        {
            PolygonCollider2D polygonCollider = gameObject.GetComponent<PolygonCollider2D>();
            if (polygonCollider)
            {
                int pathCount = polygonCollider.pathCount;
                for (int i = 0; i != pathCount; ++i)
                {
                    Vector2[] path = polygonCollider.GetPath(i);
                    for (int j = 0; j < path.Length - 1; ++j)
                    {
                        Gizmos.DrawLine(
                            transform.TransformPoint(path[j].x, path[j].y, 1.0f),
                            transform.TransformPoint(path[j + 1].x, path[j + 1].y, 1.0f));
                    }
                    // Close loop
                    if (path.Length > 1)
                    {
                        Gizmos.DrawLine(
                            transform.TransformPoint(path[path.Length - 1].x, path[path.Length - 1].y, 1.0f),
                            transform.TransformPoint(path[0].x, path[0].y, 1.0f));
                    }
                }
            }
        }
    }
#endif
}