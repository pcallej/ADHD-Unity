using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTarget : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    private Image myImage;
    public Sprite selectedSprite;
    public animalSelector animSelect;

    // Use this for initialization
    void Start () {
        myImage = GetComponent<Image>();
        string target = animSelect.selectedAnimal.transform.GetChild(0).tag;

        switch (target) {
            case "Black":
                selectedSprite = sprites[0];
                myImage.sprite = selectedSprite;
                Debug.Log("My image is black");
            break;
            case "Corgi":
                selectedSprite = sprites[1];
                myImage.sprite = selectedSprite;
                Debug.Log("My image is corgi");
                break;
            case "Fennec":
                selectedSprite = sprites[2];
                myImage.sprite = selectedSprite;
                Debug.Log("My image is fennec");
            break;
            case "Red":
                selectedSprite = sprites[3];
                myImage.sprite = selectedSprite;
                Debug.Log("My image is red");
                break;
            case "Snow":
                selectedSprite = sprites[4];
                myImage.sprite = selectedSprite;
                Debug.Log("My image is snow");
                break;
        }
    }
}
