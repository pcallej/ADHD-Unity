using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiminishBlur : MonoBehaviour {

    [SerializeField] private Difficulty difficulty;
    [SerializeField] private float maxBlur;
    [SerializeField] private float minBlur;
     private Image blurImage;
     private Material mat;


	// Use this for initialization
	void Start () {
        blurImage = GetComponent<Image>();
        mat = blurImage.material;
        mat.shader = Shader.Find("Custom/Blur");
	}
	
	// Update is called once per frame
	void Update () {
        float blur = Mathf.Lerp(maxBlur,minBlur,Time.time / 10);
        mat.SetFloat("_Radius",blur);
        if (difficulty.finished == false) {
            if (blur == minBlur)
            {
                gameObject.SetActive(false);
            }
        }
	}
}
