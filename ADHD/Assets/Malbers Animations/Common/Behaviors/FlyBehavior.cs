using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    public class FlyBehavior : StateMachineBehaviour
    {
        public float Drag = 5;
        public float DownAcceleration = 4;
        public float FallRecovery = 1.5f;

        float acceleration = 0;
        Rigidbody rb;
        Animal animal;

        float time;

        Vector3 FallVector;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rb = animator.GetComponent<Rigidbody>();
            animal = animator.GetComponent<Animal>();
            acceleration = 0;

            if (rb)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
 
                //Just recover if your coming from the fall animations
                FallVector = animator.GetCurrentAnimatorStateInfo(layerIndex).tagHash == Hash.Fall ? 
                    rb.velocity : Vector3.zero;

                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);     //Clean the Y velocity

                rb.drag = 0;
                rb.useGravity = false;
            }

            animator.applyRootMotion = true;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            time = animator.updateMode == AnimatorUpdateMode.AnimatePhysics ? Time.fixedDeltaTime : Time.deltaTime;     //Get the Time Right
           
           
            if (FallVector != Vector3.zero)                         //if last animation was falling 
            {
                animal.DeltaPosition += FallVector * time;          //Add Recovery from falling
                FallVector = Vector3.Lerp(FallVector, Vector3.zero, time * FallRecovery);
            }

            //Add more speed when going Down
            if (animal.MovementAxis.y < -0.1)
            {
                acceleration = Mathf.Lerp(acceleration, acceleration + DownAcceleration, time);
            }
            else
            {
                float a = acceleration - DownAcceleration;
                if (a < 0) a = 0;

                acceleration = Mathf.Lerp(acceleration, a, time);  //Deacelerate slowly all the acceleration you earned..
            }

            animator.transform.position = Vector3.Lerp(animator.transform.position, animator.transform.position + animator.velocity * (acceleration / 2), time);
        }
    }
}