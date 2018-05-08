using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalSelector : MonoBehaviour {

    [SerializeField] public GameObject[] animals;
    public GameObject selectedAnimal;

	// Use this for initialization
	void Start () {
        selectedAnimal = animals[Random.Range(0, animals.Length)];
        Debug.Log("Selected animal is " + selectedAnimal.gameObject.transform.GetChild(0).tag);
	}
}
