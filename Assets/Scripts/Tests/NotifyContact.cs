using UnityEngine;
using System.Collections;

public class NotifyContact : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
	
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }
    /*
    Collision2D
    collider    The incoming Collider2D involved in the collision.
    contacts    The specific points of contact with the incoming Collider2D.
    gameObject  The incoming GameObject involved in the collision.
    relativeVelocity    The relative linear velocity of the two colliding objects (Read Only).
    rigidbody   The incoming Rigidbody2D involved in the collision.
    transform   The Transform of the incoming object involved in the collision.
    */
    void OnCollisionEnter2D (Collision2D collision)
    {
        Utils.Log (name, "-> OnCollisionEnter2D with ", collision.gameObject.name, "at velocity", collision.relativeVelocity);
        
    }
	
    void OnCollisionStay2D (Collision2D collision)
    {
        // Sent each frame while the collision continues
        // Debug.Log ("NotifyOnContact -> OnCollisionStay2D: " + collision);
    }
	
    void OnCollisionExit2D (Collision2D collision)
    {
        Utils.Log (name, "-> OnCollisionExit2D: ", collision.gameObject.name);
    }
	
    void OnTriggerEnter2D (Collider2D collider)
    {
        Utils.Log (name, "-> OnTriggerEnter2D: ", collider.gameObject.name);
    }
	
    void OnTriggerStay2D (Collider2D collider)
    {
        // Sent each frame while the collision continues
        // Debug.Log ("NotifyOnContact -> OnTriggerStay2D: " + collider);
    }
	
    void OnTriggerExit2D (Collider2D collider)
    {
        Utils.Log (name, "-> OnTriggerExit2D: ", collider.gameObject.name);
    }
}
