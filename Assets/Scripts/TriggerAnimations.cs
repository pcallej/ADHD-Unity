using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimations : MonoBehaviour {

    private GameObject magic;
    private Animator anim;
    public bool magicTrigger;

    public void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    public void Update()
    {
        activateMagic(magicTrigger);
    }

    public void activateMagic(bool activate) {
        if (activate) {
            magic = gameObject.transform.GetChild(4).GetChild(1).gameObject;
            magic.SetActive(true);
            Debug.Log("magic activated");
        }
    }
}
