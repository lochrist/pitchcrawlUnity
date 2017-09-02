using UnityEngine;
using System.Collections;

public class SpawnPoint : Slot
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

#if UNITY_EDITOR

    public virtual Texture GetGizmoTexture()
    {
        return null;
    }

    void OnDrawGizmos()
    {
        Texture gizmoTexture = GetGizmoTexture();
        if (gizmoTexture)
        {
            //Gizmos.DrawGUITexture(new Rect(transform.position.x, transform.position.y, 1.0f, -1.0f), gizmoTexture);
        }
    }

#endif
}
