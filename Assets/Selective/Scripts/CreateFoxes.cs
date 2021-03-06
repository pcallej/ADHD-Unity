﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CreateFoxes : MonoBehaviour {


    [SerializeField] GameObject camera;
    [SerializeField] private float radius;
    [SerializeField] private float radiusDifference;
    [SerializeField] private int numFloors;
    private Vector3 vertical;
    [SerializeField] private Vector3 addVertical;
    [SerializeField] private Vector3 correction;
    [SerializeField] public GameObject[] foxes;
    [SerializeField] private Difficulty diffculty;
    [Inject]
    private FoxFactory factory;
    public GameObject Foxes;
    private GameObject master;
    private animalSelector animSelector;
    [HideInInspector] public int numberOfTargets; 

    private void Start()
    {
        InstantiateAnimals();
        numberOfTargets = countTargets();
        Debug.Log(numberOfTargets);
    }

    private void InstantiateAnimals() {
        Vector3 center = transform.position;
        float sum = 0;
        Foxes = new GameObject("Foxes");
        for (int i = 0; i < numFloors; i++)
        {
            for (int j = 0; j < diffculty.numAnimals; j++)
            {
                Vector3 pos = RandomCircle(center, radius + sum);
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center);
                GameObject randFox = Instantiate(foxes[Random.Range(0, foxes.Length)], pos + vertical, rot);
                randFox.transform.Rotate(correction);
                randFox.transform.SetParent(Foxes.transform);
                randFox.transform.LookAt(camera.transform);
            }
            sum += radiusDifference;
            vertical += addVertical;
        }
    }

    private Vector3 RandomCircle(Vector3 center, float radius) 
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;

        return pos;
    }

    private int countTargets()
    {
        master = GameObject.FindGameObjectWithTag("Master");
        animSelector = master.GetComponent<animalSelector>();
        int counter = 0;
        for (int i = 0; i < Foxes.transform.childCount; i++){
            if (Foxes.transform.GetChild(i).GetChild(0).tag == animSelector.selectedAnimal.transform.GetChild(0).tag) {
                counter++;
            }
        }
        return counter;
    }
}
