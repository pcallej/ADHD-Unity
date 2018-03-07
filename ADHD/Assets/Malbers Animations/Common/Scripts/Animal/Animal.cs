using UnityEngine;

namespace MalbersAnimations
{
    [RequireComponent(typeof(Animator))]
    //[RequireComponent(typeof(Rigidbody))]
    /// <summary>
    /// This will controll all Animals Motion  
    /// Version 2.0
    /// </summary>
    public partial class Animal : MonoBehaviour, iMalbersInputs , IAnimatorListener , IMDamagable
    {
        //This was left in blank Intentionally

        //Animal Variables: All variables
        //Animal Movement: All Locomotion Logic
        //Animal CallBacks: All public methods and behaviors thatic can be called outside the script
    }
}
