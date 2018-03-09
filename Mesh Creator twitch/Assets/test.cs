using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public Material mat;

	// Use this for initialization
	void Start () {
        meshCreator.createMesh(5, 10, mat);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
