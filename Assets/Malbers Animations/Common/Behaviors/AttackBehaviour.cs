using UnityEngine;

namespace MalbersAnimations
{
    /// <summary>
    /// Enable Disable the Attack Triggers on the Animal
    /// </summary>
    public class AttackBehaviour : StateMachineBehaviour
    {
        public int AttackTrigger = 1;
        [Range(0, 1)]
        public float On = 0.3f;
        [Range(0, 1)]
        public float Off = 0.6f;

        bool isOn, isOff;
        Animal animal;

        float attacktime, attackDelay;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal = animator.GetComponent<Animal>();
            animal.IsAttacking = true;
            animal.Attack1 = false;
            isOn = isOff = false;
            attackDelay = animal.attackDelay;
            attacktime = Time.time;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal.IsAttacking = true;                                  //Important Make Sure its stays true!!

            if (!isOn && (stateInfo.normalizedTime % 1) >= On)
            {
                animal.AttackTrigger(AttackTrigger);
                isOn = true;
            }

            if (!isOff && (stateInfo.normalizedTime % 1) >= Off)
            {
                animal.AttackTrigger(0);
                isOff = true;
            }

            if (attackDelay > 0)
            {
                if (Time.time - attacktime > attackDelay)
                {
                    animal.IsAttacking = false;
                }
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal.AttackTrigger(0);
            isOn = isOff = false;                   //To restore the Attack Triggers 
            animal.IsAttacking = false;             //Make Sure 
        }
    }
}