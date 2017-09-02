using UnityEngine;
using System.Collections;

public class TestCoRoutine : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator Start() {
		print ("Start me");
        StartCoroutine("DoSomething", 2.0F);
        yield return new WaitForSeconds(1);
        StopCoroutine("DoSomething");
		Utils.Log ("Do something has been stopped!");
    }
    IEnumerator DoSomething(float someParameter) {
        while (true) {
            print("DoSomething Loop");
            yield return null;
        }
    }
}
