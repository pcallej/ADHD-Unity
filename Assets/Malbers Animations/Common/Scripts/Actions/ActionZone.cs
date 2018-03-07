using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MalbersAnimations.Events;
using UnityEngine.Events;
using System;
using MalbersAnimations.Utilities;

namespace MalbersAnimations
{
    [RequireComponent(typeof(BoxCollider))]
    public class ActionZone : MonoBehaviour
    {
        static Keyframe[] K = { new Keyframe(0, 0), new Keyframe(1, 1) };

        public Actions actionsToUse;

        public bool automatic;                          //Set the Action Zone to Automatic
        public int ID;                                  //ID of the Action Zone (Value)
        public int index;                               //Index of the Action Zone (List index)
        public float AutomaticDisabled = 10f;            //is Automatic is set to true this will be the time to disable temporarly the Trigger
        public bool HeadOnly;                           //Use the Trigger for heads only
        public bool ActiveOnJump = false;


        public bool Align;                              //Align the Animal entering to the Aling Point
        public Transform AlingPoint;
        public float AlignTime = 0.5f;
        public AnimationCurve AlignCurve = new AnimationCurve(K);

        public bool AlignPos = true, AlignRot = true, AlignLookAt = false;

        protected List<Collider> animal_Colliders;
        protected Animal oldAnimal;
        public float ActionDelay = 0;

        public AnimalEvent OnEnter = new AnimalEvent();
        public AnimalEvent OnExit = new AnimalEvent();
        public AnimalEvent OnAction = new AnimalEvent();


        public static List<ActionZone> ActionZones;

        //───────AI──────────────────────────
        public float stoppingDistance = 0.5f;
        public Transform NextTarget;


        Collider ZoneCollider;

        void OnEnable()
        {
            if (ActionZones == null) ActionZones = new List<ActionZone>();

            ZoneCollider = GetComponent<Collider>();                                   //Get the reference for the collider

            ActionZones.Add(this);                                                  //Save the the Action Zones on the global Action Zone list
        }

        void OnDisable()
        {
            ActionZones.Remove(this);                                              //Remove the the Action Zones on the global Action Zone list
            if (oldAnimal)
            {
                oldAnimal.OnAction.RemoveListener(OnActionListener);
                oldAnimal.ActionID = -1;
            }
        }


        void OnTriggerEnter(Collider other)
        {
            Animal newAnimal = other.GetComponentInParent<Animal>();               //Get the animal on the entering collider
            if (!newAnimal) return;                                                //If there's no animal script found do nothing

            if (other.gameObject.layer != 20) return;                           //Just use the Colliders with the Animal Layer on it
                

            if (animal_Colliders == null)
                animal_Colliders = new List<Collider>();                              //Check all the colliders that enters the Action Zone Trigger

            if (HeadOnly && !other.name.ToLower().Contains("head")) return;     //If is Head Only and no head was found Skip

            newAnimal.ActionID = ID;

            if (animal_Colliders.Find(item => item == other) == null)                 //if the entering collider is not already on the list add it
            {
                animal_Colliders.Add(other);
            }

            if (newAnimal == oldAnimal) return;                                  //if the animal is the same do nothing (when entering two animals on the same Zone)
            else
            {
               if(oldAnimal) oldAnimal.ActionID = -1;                            //Remove the old animal and remove the Action ID
                oldAnimal = newAnimal;                                           //Set a new Animal
            }

            newAnimal.OnAction.AddListener(OnActionListener);                      //Listen when the animal activate the Action Input

            OnEnter.Invoke(newAnimal);
         

            if (automatic)       //Just activate when is on the Locomotion State if this is automatic
            {
                if (newAnimal.CurrentAnimState.tagHash == Hash.Tag_Jump && !ActiveOnJump) return;   //Dont start an automatic action if is jumping and active on jump is disabled
               
                newAnimal.SetAction(ID);
                StartCoroutine(ReEnable(newAnimal));
            }
        }

        void OnTriggerExit(Collider other)
        {
            Animal exiting_animal = other.GetComponentInParent<Animal>();
            if (!exiting_animal) return;                                            //If there's no animal script found skip all

            if (exiting_animal != oldAnimal) return;                                //If is another animal exiting the zone SKIP

            if (HeadOnly && !other.name.ToLower().Contains("head")) return;         //if is only set to head and there's no head SKIP

            if (animal_Colliders.Find(item => item == other))                       //Remove the collider from the list that is exiting the zone.
            { animal_Colliders.Remove(other); }
                
            

            if (animal_Colliders.Count == 0)                                        //When all the collides are removed from the list..
            {
                OnExit.Invoke(oldAnimal);                                           //Invoke On Exit when all animal's colliders has exited the Zone
                oldAnimal.OnAction.RemoveListener(OnActionListener);                //Remove the Method fron the Action Listener

                if (oldAnimal.ActionID == ID) oldAnimal.ActionID = -1;              //Reset the Action ID if we have the same

                oldAnimal = null;
            }
        }

        /// <summary>
        /// This will disable the Collider on the action zone
        /// </summary>
        IEnumerator ReEnable(Animal animal) //For Automatic only 
        {
            if (AutomaticDisabled > 0)
            {
                ZoneCollider.enabled = false;
                yield return null;
                yield return null;
                animal.ActionID = -1;
                yield return new WaitForSeconds(AutomaticDisabled);
                ZoneCollider.enabled = true;
            }
            oldAnimal = null;       //clean animal
            animal_Colliders = null;      //Reset Colliders
            yield return null;
        }

        public virtual void _DestroyActionZone(float time)
        {
            Destroy(gameObject, time);
        }

        /// <summary>
        /// Used for checking if the animal enables the action
        /// </summary>
        private void OnActionListener()
        {
            if (!oldAnimal) return;                            //Skip if there's no animal

            if (ActionDelay > 0)
            {
                Invoke("OnActionEvent", ActionDelay);          //Invole the Event OnAction)
            }
            else
            {
                OnAction.Invoke(oldAnimal);                    //Invole the Event OnAction
            }
           
            if (Align && AlingPoint)
            {
                IEnumerator ICo = null;

                if (AlignLookAt)
                {
                    ICo = MalbersTools.AlignLookAtTransform(oldAnimal.transform, AlingPoint, AlignTime, AlignCurve);     //Align Look At the Zone
                }
                else
                {
                    ICo = MalbersTools.AlignTransformsC(oldAnimal.transform, AlingPoint, AlignTime, AlignPos, AlignRot, AlignCurve); //Aling Transform to another transform
                }

                StartCoroutine(ICo);
            }

            StartCoroutine(CheckForCollidersOff());
        }


        protected void OnActionEvent()
        {
            OnAction.Invoke(oldAnimal);                        //Invole the Event OnAction Delayed
        }

        IEnumerator CheckForCollidersOff() //??
        {
            yield return null;      
            yield return null;          //Wait 2 frames

            if (animal_Colliders != null && animal_Colliders[0] && animal_Colliders[0].enabled == false)
            {
                oldAnimal.OnAction.RemoveListener(OnActionListener);
                oldAnimal.ActionID = -1;
                oldAnimal = null;
                animal_Colliders = null;
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (EditorAI)
            {
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, stoppingDistance);
            }
        }
#endif

        [HideInInspector] public bool EditorShowEvents = true;
        [HideInInspector] public bool EditorAI = true;
    }
}