using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : MonoBehaviour {

    [SerializeField] private GameObject targetMenu;
    [SerializeField] private GameObject menuToDisappear;

    public void TriggerMenu(bool triggermenu) {
        if (triggermenu) {
            Debug.Log("activated menu");
            menuToDisappear.SetActive(false);
            targetMenu.SetActive(true);
        }
    }
}
