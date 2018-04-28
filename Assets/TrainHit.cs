using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHit : MonoBehaviour {

    
    void OnTriggerEnter(Collider other)
    {
         switch (gameObject.tag) {
            case "crash":
                Debug.Log("Train Hit track");
                other.transform.GetComponentInParent<PathFollower>().TrainCrashed();
                break;
            case "Goal":
                Debug.Log("Train hit goal");
                other.gameObject.SetActive(false);
                break;
        }
    }
}
