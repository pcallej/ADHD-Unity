using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTarget : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    private Image myImage;

	// Use this for initialization
	void Start () {
        myImage = GetComponent<Image>();
        Sprite myTarget = sprites[Random.Range(0, sprites.Length-1)];
        myImage.sprite = myTarget;
	}
}
