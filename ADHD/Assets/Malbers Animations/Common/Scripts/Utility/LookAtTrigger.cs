using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MalbersAnimations.Utilities
{
    public class LookAtTrigger : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return;

            LookAt lookAt = other.GetComponentInParent<LookAt>();

            if (!lookAt) return;
            lookAt.active = true;
            lookAt.Target = transform;
        }

        void OnTriggerExit(Collider other)
        {
            if (other.isTrigger) return;

            LookAt lookAt = other.GetComponentInParent<LookAt>();

            if (!lookAt) return;

            lookAt.Target = null;

        }
    }
}