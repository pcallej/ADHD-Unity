using MalbersAnimations.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MalbersAnimations
{
    public class JumpBehaviour : StateMachineBehaviour
    {
        [Header("Checking Fall")]
        [Tooltip("Ray Length to check if the ground is at the same level all the time")]
        public float fallRay = 1.7f;

        [Tooltip("Terrain difference to be sure the animal will fall ")]
        public float threshold = 0.3f;

        [Tooltip("Animation normalized time to change to fall animation if the ray checks if the animal is falling ")]
        public float willFall = 0.7f;

        [Header("Jump Up Cliff")]
        public float startEdge = 0.5f;
        public float finishEdge = 0.6f;
        public float CliffRay = 0.6f;

        [Space]
        [Header("Add more Height and Distance to the Jump")]
        public float JumpMultiplier = 1;
        public float ForwardMultiplier = 1;



        float jumpPoint;
        float Rb_Y_Speed = 0;
        RaycastHit JumpRay;
        Animal animal;
        Rigidbody rb;
        Vector3 JumpControl;
        float smooth;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal = animator.GetComponent<Animal>();
            rb = animator.GetComponent<Rigidbody>();

            jumpPoint = animator.transform.position.y;
            animal.InAir(true);
            animal.SetIntID(0);

            animal.OnJump.Invoke();     //Invoke that the Animal is Jumping

            Rb_Y_Speed = 0;             //For Flying

            //rb.useGravity = false;

            smooth =  0;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            bool isInTransition = animator.IsInTransition(layerIndex);

            if (!isInTransition && JumpMultiplier > 0 && animal.JumpHeightMultiplier > 0)  //Add More Height to the Jump
            {
                rb.AddForce((Vector3.up * JumpMultiplier * 10) + (animator.transform.forward * ForwardMultiplier * 10), ForceMode.Acceleration);
            }

            Debug.DrawRay(animal.Pivot_fall, -animal.transform.up * animal.Chest_Pivot_Multiplier * fallRay, Color.red);

            //This code is execute when the animal can change to fall state if there's no future ground to land on
            if (Physics.Raycast(animal.Pivot_fall, -animal.transform.up, out JumpRay, animal.Chest_Pivot_Multiplier * fallRay, animal.GroundLayer))
            {
                if ((jumpPoint - JumpRay.point.y) <= (threshold * animal.ScaleFactor)
                    && (Vector3.Angle(JumpRay.normal, Vector3.up) < animal.maxAngleSlope))      //If if finding a lower jump point;
                {
                    animal.SetIntID(0);                                                         //Keep the INTID in 0
                    MalbersTools.DebugTriangle(JumpRay.point, 0.1f, Color.red);
                }
                else
                {
                    if (stateInfo.normalizedTime > willFall) animal.SetIntID(111);              //Set INTID to 111 to activate the FALL transition
                    MalbersTools.DebugTriangle(JumpRay.point, 0.1f, Color.yellow);
                }
            }
            else
            {
                if (stateInfo.normalizedTime > willFall) animal.SetIntID(111); //Set INTID to 111 to activate the FALL transition

                MalbersTools.DebugPlane(animal.Pivot_fall - (animal.transform.up * animal.Chest_Pivot_Multiplier * fallRay), 0.1f, Color.red);
            }

            //───────────────────────────────────────────────Get jumping on a cliff ────────────────────────────────────────────────────────────────────────────────────────────────────

            if (stateInfo.normalizedTime >= startEdge && stateInfo.normalizedTime <= finishEdge)
            {
                if (Physics.Raycast(animal.Chest_Pivot_Point, -animal.transform.up, out JumpRay, CliffRay * animal.ScaleFactor, animal.GroundLayer))
                {
                    if (Vector3.Angle(JumpRay.normal, Vector3.up) < animal.maxAngleSlope)       //Jump to a jumpable cliff not an inclined one
                    {
                        if (animal.debug)
                        {
                            Debug.DrawLine(animal.Chest_Pivot_Point, JumpRay.point, Color.black);
                            MalbersTools.DebugTriangle(JumpRay.point, 0.1f, Color.black);
                        }
                        animal.SetIntID(110);
                    }
                }
                else
                {
                    if (animal.debug)
                    {
                        Debug.DrawRay(animal.Chest_Pivot_Point, -animator.transform.up * CliffRay * animal.ScaleFactor, Color.black);
                        MalbersTools.DebugPlane(animal.Chest_Pivot_Point - (animal.transform.up * CliffRay * animal.ScaleFactor), 0.1f, Color.black);
                    }
                }
            }


            if (animal.forwardJumpControl)  //If the jump can be controlled on air
            {
                Vector3 pos = animator.transform.position;
                JumpControl = Vector3.Lerp(pos, pos - new Vector3(animator.velocity.x, 0, animator.velocity.z), Time.deltaTime);

                if (animal.MovementReleased)
                {
                    animator.transform.position = Vector3.Lerp(animator.transform.position, JumpControl, smooth);
                }


                smooth += (animal.MovementReleased ? Time.deltaTime : -Time.deltaTime) * animal.smoothJumpForward;

                smooth = Mathf.Clamp01(smooth);
            }
           

            #region if is transitioning to flying
            //If the next animation is FLY smoothly remove the Y rigidbody speed
            if (rb && isInTransition && animator.GetNextAnimatorStateInfo(layerIndex).tagHash == Hash.Fly)
            {
                float transitionTime = animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime;
                Vector3 cleanY = rb.velocity;

                if (Rb_Y_Speed < cleanY.y) Rb_Y_Speed = cleanY.y; //Get the max Y SPEED

                cleanY.y = Mathf.Lerp(Rb_Y_Speed, 0, transitionTime);

                rb.velocity = cleanY;
            }
            #endregion
        }

        //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal.SetIntID(0);

            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(layerIndex);

           
            if (rb && currentState.tagHash == Hash.Fly)                         //if the next animation is fly then clean the rigidbody velocity on the Y axis
            {
                Vector3 cleanY = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.velocity = cleanY;
            }


            if (currentState.tagHash != Hash.Fall && currentState.tagHash != Hash.Fly) //if the next state is NOT Fall or Fly set the GroundConstraints
            {
                animal.IsInAir = false;
            }
        }
    }
}