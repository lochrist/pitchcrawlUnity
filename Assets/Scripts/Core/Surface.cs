using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Surface : Prop {

    public float surfaceDrag;
    public Dictionary<GameObject, float> objectsToDragMap = new Dictionary<GameObject, float>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.rigidbody2D && !objectsToDragMap.ContainsKey (collider.gameObject)) {
            // Utils.Log (name, " change drag to:", surfaceDrag, "(", collider.gameObject.name, ")");
            objectsToDragMap[collider.gameObject] = collider.gameObject.rigidbody2D.drag;
            collider.gameObject.rigidbody2D.drag = surfaceDrag;
        }
    }
    
    void OnTriggerStay2D (Collider2D collider)
    {
        // Sent each frame while the collision continues
        // Debug.Log ("NotifyOnContact -> OnTriggerStay2D: " + collider);
    }
    
    void OnTriggerExit2D (Collider2D collider)
    {
        float originalDrag = 1.0f;
        if (objectsToDragMap.TryGetValue(collider.gameObject, out originalDrag)) {
            // Utils.Log (name, "restore drag to:", originalDrag, "(", collider.gameObject.name, ")");
            collider.gameObject.rigidbody2D.drag = originalDrag;
            objectsToDragMap.Remove(collider.gameObject);
        }
    }
}
