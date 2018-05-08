using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHit : MonoBehaviour {

    
    void OnTriggerEnter(Collider other)
    {
         switch (gameObject.tag) {
            case "crash":
                other.transform.GetComponentInParent<PathFollower>().TrainCrashed();
                break;
            case "Goal":
                other.transform.GetComponentInParent<PathFollower>().TrainSuccess();
                break;
        }
    }
}
