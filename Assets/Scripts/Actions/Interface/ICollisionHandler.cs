using UnityEngine;
using System.Collections;

public interface ICollisionHandler {

    void HandleCollisionEnter2D (Collision2D collision);
    void HandleCollisionStay2D (Collision2D collision);
    void HandleCollisionExit2D (Collision2D collision);

    void HandleTriggerEnter2D (Collider2D collider);
    void HandleTriggerStay2D (Collider2D collider);
    void HandleTriggerExit2D (Collider2D collider);
}
