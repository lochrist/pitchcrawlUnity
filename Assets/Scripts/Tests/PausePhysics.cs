using UnityEngine;
using System.Collections;

public class PausePhysics : MonoBehaviour {

    public bool pausePhysics = false;
	
	// Update is called once per frame
	void Update () {
        if (pausePhysics && Time.timeScale > 0) {
            Time.timeScale = 0.0f;
        } else if (!pausePhysics && Time.timeScale < 1.0f) {
            Time.timeScale = 1.0f;
        }
	}
}
