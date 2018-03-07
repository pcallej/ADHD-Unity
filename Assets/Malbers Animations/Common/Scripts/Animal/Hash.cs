using UnityEngine;

namespace MalbersAnimations
{
    /// <summary>
    /// Faster way to work with Animator Controller Parameters
    /// </summary>
    public static class Hash 
    {
        public static int Vertical = Animator.StringToHash("Vertical");
        public static int Horizontal = Animator.StringToHash("Horizontal");
        public static int UpDown = Animator.StringToHash("UpDown");

        public static int Stand = Animator.StringToHash("Stand");
        public static int Grounded = Animator.StringToHash("Grounded");

        public static int _Jump = Animator.StringToHash("_Jump");

        public static int Dodge = Animator.StringToHash("Dodge");
        public static int Fall = Animator.StringToHash("Fall");
        public static int Type = Animator.StringToHash("Type");


        public static int Slope = Animator.StringToHash("Slope");

        public static int Shift = Animator.StringToHash("Shift");

        public static int Fly = Animator.StringToHash("Fly");

        public static int Attack1 = Animator.StringToHash("Attack1");
        public static int Attack2 = Animator.StringToHash("Attack2");

        public static int Death = Animator.StringToHash("Death");

        public static int Damaged = Animator.StringToHash("Damaged");
        public static int Stunned = Animator.StringToHash("Stunned");

        public static int IDInt = Animator.StringToHash("IDInt");
        public static int IDFloat = Animator.StringToHash("IDFloat");

        public static int Swim = Animator.StringToHash("Swim");
        public static int Underwater = Animator.StringToHash("Underwater");

        public static int IDAction = Animator.StringToHash("IDAction");
        public static int Action = Animator.StringToHash("Action");


        public static int Null = Animator.StringToHash("Null");
        public static int Empty = Animator.StringToHash("Empty");



        public static int Locomotion = Animator.StringToHash("Locomotion");
        public static int Tag_Idle = Animator.StringToHash("Idle");
        public static int Tag_Recover = Animator.StringToHash("Recover");
        public static int Tag_Sleep = Animator.StringToHash("Sleep");
        public static int Tag_Attack = Animator.StringToHash("Attack");
        public static int Tag_JumpEnd = Animator.StringToHash("JumpEnd");
        public static int Tag_JumpStart = Animator.StringToHash("JumpStart");
        public static int Tag_Jump = Animator.StringToHash("Jump");
        public static int Tag_SwimJump = Animator.StringToHash("SwimJump");
        public static int Tag_NoAlign = Animator.StringToHash("NoAlign");


        public static int State = Animator.StringToHash("State");
        public static int Stance = Animator.StringToHash("Stance");
        public static int Mode = Animator.StringToHash("Mode");



        //---------------------------HAP-----------------------------------------


        public readonly static int IKLeftFoot = Animator.StringToHash("IKLeftFoot");
        public readonly static int IKRightFoot = Animator.StringToHash("IKRightFoot");

        public readonly static int Mount = Animator.StringToHash("Mount");
        public readonly static int MountSide = Animator.StringToHash("MountSide");

        public readonly static int Tag_Mounting= Animator.StringToHash("Mounting");
        public readonly static int Tag_Unmounting = Animator.StringToHash("Unmounting");

    }

    /// <summary>
    /// Current Ability the Animal is doing
    /// </summary>
    public static class State
    {
        public static int Unknown = -1;
        public static int Idle = 0;

        public static int Locomotion = 1;
        public static int Jump = 2;
        public static int Climb = 3;
        public static int Fall = 4;
        public static int Recover = 5;

        public static int Action = 6;
        public static int Swim = 7;
        public static int UnderWater = 8;
        public static int Fly = 10;
        public static int Stun = 11;
        public static int Death = 12;
        public static int Rebirth = 13;
    }

    /// <summary>
    /// Actions can be used with the state and the stances
    /// </summary>
    public static class Mode
    {
        public static int None = 0;
        public static int Shift = 1;
        public static int Attack1 = 2;
        public static int Attack2 = 3;
        public static int Damaged = 4;
        public static int Dodge = 5;
        public static int Defend = 6;

        public static int isAttacking1 = 11;
        public static int isAttacking2 = 12;
        public static int isTakingDamage = 13;
        public static int isDodging = 14;
        public static int isDefending = 15;
    }

    /// <summary>
    /// Current Stance the animal is on
    /// </summary>
    public static class Stance
    {
        public static int Default = 0;
        public static int Sneak = 1;
        public static int Combat = 2;
        public static int Injured = 3;
    }
}