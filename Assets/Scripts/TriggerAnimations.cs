using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimations : MonoBehaviour {

    private GameObject magic;
    private Animator anim;
    public bool magicTrigger;
    private bool isTarget;

    public void Start() {
        anim = gameObject.GetComponent<Animator>();
        GameObject gameMaster = GameObject.FindGameObjectWithTag("Master");
        animalSelector animSelector = gameMaster.GetComponent<animalSelector>();
        if (gameObject.tag == animSelector.selectedAnimal.transform.GetChild(0).tag) {
            isTarget = true;
        }
    }

    public void Update()
    {
        activateMagic(magicTrigger);
    }

    public void activateMagic(bool activate) {
        if (isTarget) {
            if (activate){
                magic = gameObject.transform.GetChild(4).GetChild(1).gameObject;
                magic.SetActive(true);
                Debug.Log("magic activated");
            }
        }
    }
}
