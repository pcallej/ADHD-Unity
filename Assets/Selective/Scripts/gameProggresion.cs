using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameProggresion : MonoBehaviour {

    public GameObject animalFactory;
    public CreateFoxes createAnimals;
    public int numTargets;
    public GameObject endScreen;
    public int counter;

	// Use this for initialization
	void Start () {
        counter = 0;
        createAnimals = animalFactory.GetComponent<CreateFoxes>();
	}
	
	// Update is called once per frame
	void Update () {
        if (counter == createAnimals.numberOfTargets) {
            endScreen.SetActive(true);
        }
	}
}
