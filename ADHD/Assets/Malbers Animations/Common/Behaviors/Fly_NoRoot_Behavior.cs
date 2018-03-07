using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    //Fly Animations are inPlace
    public class Fly_NoRoot_Behavior : StateMachineBehaviour
    {
        [Range(0, 90)]
        public float Bank = 30;
        public float UpAxisSmooth = 2;
        [Range(0, 90)]
        public float Ylimit = 87;

        public float Drag = 5;

        [Space]

        public float DownAcceleration = 4;
        public float FallRecovery = 1.5f;

        float acceleration = 0;
        Rigidbody rb;
        Animal animal;
        protected Transform transform;
        protected Quaternion DeltaRotation;
        protected float Shift;
        protected float Direction;

        float time;

        Vector3 FallVector;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rb = animator.GetComponent<Rigidbody>();
            animal = animator.GetComponent<Animal>();

            if (animator.applyRootMotion) animator.applyRootMotion = false;
            transform = animator.transform;
            DeltaRotation = transform.rotation;

            acceleration = 0;

            if (rb)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;

                //Just recover if your coming from the fall animations
                FallVector = animator.GetCurrentAnimatorStateInfo(layerIndex).tagHash == Hash.Fall ?
                    rb.velocity : Vector3.zero;

                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);     //Clean the Y velocity

                rb.useGravity = false;
                rb.drag = Drag;
            }
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            time = animator.updateMode == AnimatorUpdateMode.Normal ? Time.deltaTime : Time.fixedDeltaTime;

            if (animal.Jump) animal.Down = false;

            transform.rotation = DeltaRotation;                                                    //Reset the Rotation before rotating

            Shift = Mathf.Lerp(Shift,animal.Shift ? 2 : 1, animal.flySpeed.lerpPosition * time);    //Calculate the Shift

            animal.YAxisMovement(UpAxisSmooth, time);                                               //Calculate YAxis Movement

            Direction = Mathf.Lerp(Direction, !animal.Shift ? animal.Direction : animal.Direction / 2, time * animal.flySpeed.lerpRotation);


           // DeltaRotation *= Quaternion.Euler(0, Direction * animal.flySpeed.rotation * (animal.MovementAxis.z >= 0 ? 1 : -1), 0);          //Rotation for the Turn;  
            transform.rotation *= Quaternion.Euler(0, Direction * animal.flySpeed.rotation * (animal.MovementAxis.z >= 0 ? 1 : -1), 0);

            DeltaRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * rb.rotation;

            float Up = animal.MovementAxis.y;

            if (animal.MovementAxis.z < 0) Up = 0;                                                      //Remove UP DOWN MOVEMENT while going backwards

            float forwardMovement = Mathf.Clamp(animal.Speed, -1, 1);

            Vector3 forward = (transform.forward * forwardMovement) + (transform.up * Up);       //Calculate the Direction to Move

           if (forward.magnitude>1) forward.Normalize();                                                                        //Remove extra Speed


            animal.DeltaPosition += forward * animal.flySpeed.position * Shift * (time * animal.flySpeed.lerpPosition) * (animal.MovementForward < 0 ? 0.5f:1);

           

            if (forward.magnitude > 0.001)
            {
                float angle = 90 - Vector3.Angle(Vector3.up, forward);

                float smooth = Mathf.Max(Mathf.Abs(animal.MovementAxis.y), Mathf.Abs(forwardMovement));

                transform.Rotate(Mathf.Clamp(-angle, -Ylimit, Ylimit) * smooth, 0, 0, Space.Self);          //Rotate to the direction is Flying
            }

            // transform.RotateAroundl(Vector3.forward, -Bank * (Direction));
            Vector3 forwardDir =new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

            transform.RotateAround(transform.position, forwardDir, -Bank * Mathf.Clamp(Direction,-1,1)); //Rotation Bank

            // transform.localRotation *= Quaternion.Euler(0, 0, -Bank * Direction);                     //Rotation Bank
            //  transform.Rotate(0, 0, -Bank * (Direction), Space.Self);                                //Rotation Bank


            if (FallVector != Vector3.zero)         //if last animation was falling 
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

            animal.DeltaPosition += forward * (acceleration * time);
        }
    }
}