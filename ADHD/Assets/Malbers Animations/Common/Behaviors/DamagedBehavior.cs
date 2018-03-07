using UnityEngine;

namespace MalbersAnimations
{
    public class DamagedBehavior : StateMachineBehaviour
    {
        int Side = 0;
        public bool DirectionalDamage = true;

        //Calculate the Direction from where is coming the hit and plays hits respective animation.
        override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            Animal animal = animator.GetComponent<Animal>();

            animal.Damaged = false;                                 // Set Damage to false so it wont get "pressed"

            if (!DirectionalDamage) return;

            Vector3 hitdirection = animal.HitDirection;
            Vector3 forward = animator.transform.forward;
            bool left = true;

            hitdirection.y = 0;
            forward.y = 0;

            float angle = Vector3.Angle(forward, hitdirection);                             //Get The angle

            if (Vector3.Dot(animal.transform.right, animal.HitDirection) > 0) left = false;  //Calculate witch directions comes the hit

            if (left)
            {
                if (angle > 0 && angle <= 60) Side = 3;
                else if (angle > 60 && angle <= 120) Side = 2;
                else if (angle > 120 && angle <= 180) Side = 1;
            }
            else
            {
                if (angle > 0 && angle <= 60) Side = -3;
                else if (angle > 60 && angle <= 120) Side = -2;
                else if (angle > 120 && angle <= 180) Side = -1;
            }
            animator.SetInteger(Hash.IDInt, Side);
        }
    }
}