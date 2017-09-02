using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SurfaceModifier : MonoBehaviour
{
    public float drag;

    class BodyDesc {
        public BodyDesc(float od, Rigidbody2D b) {
            originalDrag = od;
            body = b;
        }

        public float originalDrag;
        public Rigidbody2D body;
    }

    List<BodyDesc> slidingBodies = new List<BodyDesc>();

    // Use this for initialization
    void Start ()
    {
	
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }

    BodyDesc FindBody(GameObject go) {
        return slidingBodies.Find (desc => desc.body == go.rigidbody2D);
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.rigidbody2D && FindBody (collider.gameObject) == null) {
            Utils.Log (name, " change drag to:", drag, "(", collider.gameObject.name, ")");
            BodyDesc desc = new BodyDesc(collider.gameObject.rigidbody2D.drag, collider.gameObject.rigidbody2D);
            collider.gameObject.rigidbody2D.drag = drag;
            slidingBodies.Add (desc);
        }
    }
    
    void OnTriggerStay2D (Collider2D collider)
    {
        // Sent each frame while the collision continues
        // Debug.Log ("NotifyOnContact -> OnTriggerStay2D: " + collider);
    }
    
    void OnTriggerExit2D (Collider2D collider)
    {
        if (collider.gameObject.rigidbody2D) {
            var bodyDesc = FindBody (collider.gameObject);
            if (bodyDesc != null) {
                Utils.Log (name, "restore drag to:", bodyDesc.originalDrag, "(", collider.gameObject.name, ")");
                collider.gameObject.rigidbody2D.drag = bodyDesc.originalDrag;
                slidingBodies.Remove(bodyDesc);
            }
        }
    }
}
