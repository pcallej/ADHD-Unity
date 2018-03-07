using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    public class FallBehavior : StateMachineBehaviour
    {
        RaycastHit FallRay;
        
        [Tooltip("The Lower Fall animation will set to 1 if this distance the current distance to the ground")]
        public float LowerDistance;
        float animalFloat;
        Animal animal;
        Rigidbody rb;
        float MaxHeight; //this is to store the max Y heigth before falling

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal = animator.GetComponent<Animal>();
            rb = animator.GetComponent<Rigidbody>();

            animal.SetIntID(1);
            animal.IsInAir = true;                                          //the  Animal is on the air
            animator.SetFloat(Hash.IDFloat, 1);

            //Vector3 inertia = animator.velocity;
            //inertia.y = 0;

            MaxHeight = 0; //Resets MaxHeight

            animator.applyRootMotion = false;
            if (rb)
            {
                rb.drag = 0;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.useGravity = true;
                rb.isKinematic = false;
             //   rb.AddForce(inertia, ForceMode.VelocityChange);
            }
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.IsInTransition(0) && rb && rb.constraints != (RigidbodyConstraints) 112)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }

            if (!rb)
            {
                animal.transform.position += Physics.gravity * Time.deltaTime;
            }

            if (animator.applyRootMotion == true) animator.applyRootMotion = false; //In case the Appylroot motion gets reactive
         

            if (Physics.Raycast(animator.transform.position, -animal.transform.up, out FallRay, 100, animal.GroundLayer))
            {
                if (MaxHeight < FallRay.distance)
                {
                    MaxHeight = FallRay.distance; //get the lower Distance 
                }

                //Blend between fall animations ... Higher 1 one animation
                animalFloat = Mathf.Lerp(animalFloat, 
                    Mathf.Lerp(1, 0, (MaxHeight - FallRay.distance) / (MaxHeight - LowerDistance)),
                    Time.deltaTime * 20f);

                animator.SetFloat(Hash.IDFloat, animalFloat);
            }
        }
    }
}