using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    /// <summary>
    /// This Behavior Updates and resets all parameters to their original state
    /// </summary>
    public class RecoverBehavior : StateMachineBehaviour
    {
        public float smoothness = 10;
        [Tooltip("")]
        public float MaxDrag = 3;
        public bool stillContraints = true;
        public bool Landing = true;
        Animal animal;
        Rigidbody rb;
        float time;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal = animator.GetComponent<Animal>();                           //Get Reference for Animal
            rb = animator.GetComponent<Rigidbody>();                            //Get Reference for Rigid Body
            animator.applyRootMotion = false;

            if (Landing)
            {
                animal.IsInAir = false;
                if (rb && stillContraints) rb.constraints = Animal.Still_Constraints;
            }
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            time = animator.updateMode == AnimatorUpdateMode.AnimatePhysics ? Time.fixedDeltaTime : Time.deltaTime;     //Get the Time Right


            if (Landing) 
            {
                animal.IsInAir = false;
            }
            else
            {
                rb.useGravity = false;
            }

            if (stateInfo.normalizedTime < 0.9f)   //Smooth Stop when RecoverFalls
            {
               if (rb) rb.drag = Mathf.Lerp(rb.drag, MaxDrag, time * smoothness);
            }
            else
            {
                animator.applyRootMotion = true;
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.applyRootMotion != true)
                animator.applyRootMotion = true;

            if (rb) rb.drag = 0; //Reset the Drag
        }
    }
}