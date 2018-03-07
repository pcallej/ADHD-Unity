using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations
{
    public class ActivateJump : MonoBehaviour
    {
        // Use this for initialization
        private void OnTriggerEnter(Collider other)
        {
            Animal animal = other.GetComponentInParent<Animal>();
            if (animal) animal.SetJump();
        }
    }
}