using UnityEngine;

namespace MalbersAnimations
{
    public class FlyDodgeBehaviour : StateMachineBehaviour
    {
        public bool InPlace;
        Vector3 momentum;       //To Store the velocity that the animator had before entering this animation state
        Rigidbody rb;
        float time;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rb = animator.GetComponent<Rigidbody>();

            momentum = InPlace ? rb.velocity : animator.velocity;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            time = animator.updateMode == AnimatorUpdateMode.AnimatePhysics ? Time.fixedDeltaTime : Time.deltaTime;     //Get the Time Right

            animator.transform.position = Vector3.Lerp(animator.transform.position, animator.transform.position + momentum, time);
        }
    }
}