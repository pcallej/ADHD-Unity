using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {


    [Range(-1f, 1f)]
    [SerializeField] private int rotationWay;
    [SerializeField] private float rotationSpeed; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(new Vector3(0f,rotationSpeed,0f) * rotationWay);
	}
}
