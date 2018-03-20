using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour {

    public Difficulty difficulty;
    public float targetTime;
	
	// Update is called once per frame
	void Update () {
        targetTime = difficulty.timer;
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f) {
            difficulty.finished = true;
        }
	}
}
