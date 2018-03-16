using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piegraph : MonoBehaviour {

    public float[] values;
    public Color[] wedgeColors;
    public Image wedgePrefab;

	// Use this for initialization
	void Start () {
        makeGraph();
	}

    void makeGraph() {
        float total = 0f;
        float zRotation = 0f;

        for (int i = 0; i < values.Length; i++) {
            total += values[i];
        }

        for (int i = 0; i < values.Length; i++) {
            Image newWedge = Instantiate(wedgePrefab) as Image;
            newWedge.transform.SetParent(transform, false);
            newWedge.color = wedgeColors[i];
            newWedge.fillAmount = values[i] / total;
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,zRotation));
            zRotation -= newWedge.fillAmount * 360f;
        }
    }
}
