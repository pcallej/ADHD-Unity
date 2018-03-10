using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFoxes : MonoBehaviour {


    [SerializeField] private float radius;
    [SerializeField] private GameObject[] foxes;

    void Start(){
        Vector3 center = transform.position;
        for (int i = 0; i < foxes.Length; i++) {
            Vector3 pos = RandomCircle(center, radius);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center);
            Instantiate(foxes[Random.Range(0, foxes.Length)], pos, rot);
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius) {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;

        return pos;
    }
}
