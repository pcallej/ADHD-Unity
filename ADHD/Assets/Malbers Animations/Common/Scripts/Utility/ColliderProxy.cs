using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using MalbersAnimations.Events;

namespace MalbersAnimations.Utilities
{
    /// <summary>
    /// This is used when the collider is in a different gameObject and you need to check the Collider Events
    /// Create this component at runtime and subscribe to the UnityEvents
    /// </summary>
    public class ColliderProxy : MonoBehaviour
    {
        [Header("This script requires a Collider")]
        [Space]
        public ColliderEvent OnTrigger_Enter = new ColliderEvent();
        public ColliderEvent OnTrigger_Stay = new ColliderEvent();
        public ColliderEvent OnTrigger_Exit = new ColliderEvent();
        public CollisionEvent OnCollision_Enter = new CollisionEvent();
        public CollisionEvent OnCollision_Stay = new CollisionEvent();
        public CollisionEvent OnCollision_Exit = new CollisionEvent();


        void OnTriggerStay(Collider other)
        {

            OnTrigger_Stay.Invoke(other);
        }

        void OnTriggerEnter(Collider other)
        {
            OnTrigger_Enter.Invoke(other);
        }

        void OnTriggerExit(Collider other)
        {
            OnTrigger_Exit.Invoke(other);
        }

        void OnCollisionEnter(Collision collision)
        {
            OnCollision_Enter.Invoke(collision);
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            OnCollision_Stay.Invoke(collisionInfo);
        }

        void OnCollisionExit(Collision collisionInfo)
        {
            OnCollision_Exit.Invoke(collisionInfo);
        }
    }
}