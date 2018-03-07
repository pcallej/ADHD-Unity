    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations
{
    //Controls the movement while underwater
    public class UnderWaterBehaviour : StateMachineBehaviour {

        [Range(0,90)]
        public float Bank;
        public float upDownSmoothness = 2;
        [Range(0, 90)]
        public float Ylimit = 87;

        protected Rigidbody rb;
        protected Animal animal;
        protected Transform transform;
        protected Quaternion DeltaRotation;
        protected float UnderWaterShift;
        protected float time;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rb = animator.GetComponent<Rigidbody>();
            animal = animator.GetComponent<Animal>();

            if (animator.applyRootMotion) animator.applyRootMotion = false;
            transform = animator.transform;
            DeltaRotation = transform.rotation;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            if (!animal.CanGoUnderWater) return;
            rb.drag = 100;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            time = animator.updateMode == AnimatorUpdateMode.Normal ? Time.deltaTime : Time.fixedDeltaTime;

            if (!animal.CanGoUnderWater) return;
            if (!animal.Underwater) return;

            if (animal.Jump) animal.Down = false;

            if (animator.applyRootMotion) animator.applyRootMotion = false;
     

            rb.useGravity = false;

            transform.rotation = DeltaRotation;                         //Store the Rotation before rotating

            if (animal.Shift)
            {
                UnderWaterShift = Mathf.Lerp(UnderWaterShift, 2f, animal.underWaterSpeed.lerpPosition * Time.fixedDeltaTime);
            }
            else
            {
                UnderWaterShift = Mathf.Lerp(UnderWaterShift, 1f, animal.underWaterSpeed.lerpPosition * Time.fixedDeltaTime);
            }

            animal.YAxisMovement(upDownSmoothness, time);


            transform.Rotate(transform.up,Mathf.Clamp( animal.Direction,-1,1) * animal.underWaterSpeed.rotation);         //Rotate while going forward

            float Up = animal.MovementAxis.y;

            if (animal.MovementAxis.z < 0) Up = 0;                                                      //Remove Rotations When going backwards

            Vector3 forward = (transform.forward * animal.Speed) + (transform.up * Up);                 //Calculate the Direction to Move

        
            if (forward.magnitude > 1) forward.Normalize();                                             //Remove extra Speed

            transform.position = Vector3.Lerp(transform.position, transform.position + (forward * animal.underWaterSpeed.position * UnderWaterShift), Time.fixedDeltaTime * animal.underWaterSpeed.lerpPosition); //Move it

           // DeltaRotation = transform.rotation;
            DeltaRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * rb.rotation;

            if (forward.magnitude > 0.001)
            {
                float angle = 90 - Vector3.Angle(Vector3.up, forward);

                float smooth = Mathf.Max(Mathf.Abs(animal.MovementAxis.y), Mathf.Abs(animal.Speed));

                transform.Rotate(Mathf.Clamp(-angle, -Ylimit, Ylimit) * smooth, 0, 0, Space.Self);          //Rotate to the direction is swimming
            }

            transform.Rotate(0, 0, -Bank * animal.Direction, Space.Self);        //Rotation Bank

            CheckExitUnderWater();
        }

        protected void CheckExitUnderWater()
        {
            //To Get Out of the Water---------------------------------
            RaycastHit UnderWaterHit;

            Vector3 origin = transform.position + new Vector3(0, (animal.height - animal.waterLine) * animal.ScaleFactor, 0);

            if (Physics.Raycast(origin, -Vector3.up, out UnderWaterHit, animal.ScaleFactor, LayerMask.GetMask("Water")))
            {
                if (!animal.Down)
                {
                    animal.Underwater = false;
                    animal.Anim.applyRootMotion = true;
                    rb.useGravity = true;
                    rb.drag = 0;
                    rb.constraints = Animal.Still_Constraints;
                    animal.MovementAxis = new Vector3(animal.MovementAxis.x, 0, animal.MovementAxis.z);
                }
            }
        }
    }
}