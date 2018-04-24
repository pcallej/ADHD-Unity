using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateTrack : MonoBehaviour {

    public bool rotateTrue;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space)) {
            gameObject.transform.Rotate(new Vector3(0f, 90f, 0f));
        }
        RotateTrack(rotateTrue);
	}

    public void RotateTrack(bool rotate) {
        if (rotate) {
            gameObject.transform.Rotate(new Vector3(0f, 90f, 0f));
        }
    }
}
