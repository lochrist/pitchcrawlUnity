using UnityEngine;
using System.Collections;

public class TestColliderInteractionHelper : MonoBehaviour {

    bool isFirstDrag = true;
	// Use this for initialization
    void Awake () {
        Utils.Log ("Helper: Awake");
        isFirstDrag = true;
    }

	void Start () {
        Utils.Log ("Helper: Start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        Utils.Log ("Helper: on mousedown", Input.mousePosition);
    }

    void OnMouseDrag() {
        if (isFirstDrag) {
            Utils.Log ("Helper: on OnMouseDrag");
            isFirstDrag = false;
        }
    }

    void OnMouseUp() {
        Utils.Log ("Helper: on OnMouseUp - > Stopping");

        GameObject.Destroy (this);
    }
}
