using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MalbersAnimations
{
    /// All Callbacks in here
    public partial class Animal
    {
        int ToogleAmount = 4; //This is used for Enabling and disabling States

        public virtual void OnAnimatorBehaviourMessage(string message, object value)
        {
            this.InvokeWithParams(message, value);
        }

        /// <summary>
        /// Find the direction hit vector and send it to the Damage Behavior with DamageValues
        /// </summary>
        public virtual void getDamaged(DamageValues DV)
        {
            if (Death) return;                                      //skip if is Death.
            if (isTakingDamage) return;                             //If is already taking damage skip...
            if (inmune) return;                                     //skip if is imnune.

            float damageTaken = DV.Amount - defense;
            OnGetDamaged.Invoke(damageTaken);
            life = life - damageTaken;                              //Remove some life

            actionID = -1;                                          //If it was doing an action Stop!;

            if (life > 0)                                           //If I have some life left play the damage Animation
            {
                damaged = true;                                     //Activate the damage so it can be seted on the Animator
                StartCoroutine(IsTakingDamageTime(damageDelay));    //Prevent to take other hit after a time.

                _hitDirection = DV.Direction;
            }
            else
            {
                Death = true;
            }
        }

        /// Find the direction hit vector and send it to the Damage Behavior without DamageValues
        public virtual void getDamaged(Vector3 Mycenter, Vector3 Theircenter, float Amount = 0)
        {
            DamageValues DV = new DamageValues(Mycenter - Theircenter, Amount);
            getDamaged(DV);
        }

        //Coroutine to avoid been hit and play damage animation twice
        IEnumerator IsTakingDamageTime(float time)
        {
            isTakingDamage = true;
            yield return new WaitForSeconds(time);
            isTakingDamage = false;
        }


        /// <summary>
        /// Activate Attack triggers 
        /// </summary>
        /// <param name="triggerIndex"></param>
        public virtual void AttackTrigger(int triggerIndex)
        {
            if (triggerIndex == -1)                         //Enable all Attack Triggers
            {
                foreach (var trigger in Attack_Triggers)
                {
                    trigger.Collider.enabled = true;
                    trigger.gameObject.SetActive(true);
                }
                return;
            }

            if (triggerIndex == 0)                          //Disable all Attack Triggers
            {
                foreach (var trigger in Attack_Triggers)
                {
                    trigger.Collider.enabled = false;
                    trigger.gameObject.SetActive(false);
                }

                return;
            }


            List<AttackTrigger> Att_T =
                Attack_Triggers.FindAll(item => item.index == triggerIndex);   //Enable just a trigger with an index

            if (Att_T != null)
            {
                foreach (var trigger in Att_T)
                {
                    trigger.Collider.enabled = true;
                    trigger.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Activate a random Attack
        /// </summary>
        public virtual void SetAttack()
        {
            activeAttack = -1;
            Attack1 = true;
        }

        /// <summary>
        /// It will set the Loop value for animation that requires looping
        /// </summary>
        public virtual void SetLoop(int cycles)
        {
            Loops = cycles;
        }

        /// <summary>
        /// Activate an Attack by his Animation State IntID Transition
        /// </summary>
        public virtual void SetAttack(int attackID)
        {
            activeAttack = attackID;
            Attack1 = true;
        }

        /// <summary>
        /// Enable/Disable Attack1
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetAttack(bool value)
        {
            Attack1 = value;
        }

        /// <summary>
        /// Activate the 2nd Attack
        /// </summary>
        public virtual void SetSecondaryAttack()
        {
            if (hasAttack2)
            {
                StartCoroutine(ToogleAttack2());
            }

        }

        /// <summary>
        /// Enable Attack2 3 times in case the Change Attack2 gets disable to quickly 
        /// </summary>
        IEnumerator ToogleAttack2()
        {
            for (int i = 0; i < ToogleAmount; i++)
            {
                Attack2 = true;
                yield return null;
            }
            Attack2 = false;
        }

        /// <summary>
        /// Returns the if the Next or Current Animator State is tagged: tag
        /// </summary>
        public virtual bool RealAnimatorState(string tag, int layerIndex = 0)
        {
            if (layerIndex == 0)
            {
                return NextAnimState.IsTag(tag) || CurrentAnimState.IsTag(tag);
            }
            else
            {
                return Anim.GetNextAnimatorStateInfo(layerIndex).IsTag(tag) || Anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tag);
            }
        }

        /// <summary>
        /// Returns the if the Next or Current Animator State is tagged: Tag
        /// </summary>
        public virtual bool RealAnimatorState(int tag, int layerIndex = 0)
        {
            if (layerIndex == 0)
            {
                return NextAnimState.tagHash == tag || CurrentAnimState.tagHash == tag;
            }
            else
            {
                return Anim.GetNextAnimatorStateInfo(layerIndex).tagHash == tag || Anim.GetCurrentAnimatorStateInfo(layerIndex).tagHash == tag;
            }
        }


        /// <summary>
        /// True if the Current or Next State of the Animator have any of the given tags
        /// </summary>
        public virtual bool RealAnimationTag(params int[] AnimsTags)
        {
            for (int i = 0; i < AnimsTags.Length; i++)
            {
                if (Anim.GetNextAnimatorStateInfo(0).tagHash == AnimsTags[i] || Anim.GetCurrentAnimatorStateInfo(0).tagHash == AnimsTags[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// True if the Current State of the Animator have any of the given tags
        /// </summary>
        public virtual bool CurrentAnimation(params int[] AnimsTags)
        {
            for (int i = 0; i < AnimsTags.Length; i++)
            {
                if (Anim.GetCurrentAnimatorStateInfo(0).tagHash == AnimsTags[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Set the Parameter Int ID to a value and pass it also to the Animator
        /// </summary>
        public void SetIntID(int value)
        {
            IDInt = value;
            Anim.SetInteger(Hash.IDInt, IDInt);         //Update the Animator
        }

        public void SetFloatID(float value)
        {
            IDFloat = value;
            Anim.SetFloat(Hash.IDFloat, IDFloat);         //Update the Animator
        }

        /// <summary>
        /// Set a Random number to ID Int , that work great for randomly Play More animations
        /// </summary>
        protected void SetIntIDRandom(int range)
        {
            IDInt = Random.Range(1, range + 1);
        }

        /// <summary>
        /// This will check is the Animal is in any Jump State
        /// </summary>
        public bool IsJumping
        {
           get { return RealAnimatorState(Hash.Tag_Jump); }
        }

        /// <summary>
        /// Toggle  the RigidBody Constraints
        /// </summary>
        public virtual void StillConstraints(bool active)
        {
            if (_RigidBody)
            {
                _RigidBody.constraints = active ? Still_Constraints : RigidbodyConstraints.FreezeRotation;

                if (active) _RigidBody.useGravity = true;           //Activate gravity
            }
        }

        List<Collider> _col_ = new List<Collider>();       //Colliders to disable with animator

        /// <summary>
        /// Enable/Disable All Colliders on the animal. Avoid the Triggers
        /// </summary>
        public virtual void EnableColliders(bool active)
        {
            if (!active)
            {
                _col_ = GetComponentsInChildren<Collider>(false).ToList();          //Get all the Active colliders

                List<Collider> coll = new List<Collider>();

                foreach (var item in _col_)
                {
                    if (!item.isTrigger && item.enabled) coll.Add(item);            //Remove all disabled colliders and all triggers
                }
                _col_ = coll;
            }

            foreach (Collider item in _col_) { item.enabled = active; }
          
            if (active) _col_ = new List<Collider>();
        }

        /// <summary>
        /// Sets the gravity of the RigidBody
        /// </summary>
        public virtual bool Gravity
        {
          set { if (_RigidBody) _RigidBody.useGravity = value; }
        }

        /// <summary>
        /// Set the animal if is in air. 
        /// True: it will deactivate the Rigidbody constraints. 
        /// False: will freeze all rotations and Y position on the rigidbody.
        /// </summary>
        public virtual void InAir(bool active)
        {
            isInAir = active;
            StillConstraints(!active);
        }

        /// <summary>
        /// Activate the Jump and deactivate it 2 frames later
        /// </summary>
        public virtual void SetJump()
        {
            StartCoroutine(ToggleJump());
        }

        /// <summary>
        /// Set an Action using their Action ID (Find the IDs on the Animator Actions Transitions)
        /// </summary>
        /// <param name="ID"></param>
        public virtual void SetAction(int ID)
        {
            actionID = ID;
            Action = true;
        }

        /// <summary>
        /// Set an Action using their Action ID (Find the IDs on the Animator Actions Transitions)
        /// </summary>
        /// <param name="actionName">Name of the Animation State</param>
        public virtual void SetAction(string actionName)
        {
            if (Anim.HasState(0, Animator.StringToHash(actionName)))
            {
                if (CurrentAnimState.tagHash != Hash.Action && actionID <= 0)            //Don't play an action if you are already making one and if is on a Zone
                {
                    Anim.CrossFade(actionName, 0.1f, 0);
                }
            }
            else
            {
                Debug.LogWarning("The animal does not have an action called "+ actionName);
            }

        }

        /// <summary>
        /// Set the Stun to true for a time
        /// </summary>
        public virtual void SetStun(float time)
        {
            StartCoroutine(ToggleStun(time));
        }

        /// <summary>
        /// Disable this Script and MalbersInput Script if it has it.
        /// </summary>
        public virtual void DisableAnimal()
        {
            enabled = false;
            MalbersInput MI = GetComponent<MalbersInput>();
            if (MI) MI.enabled = false;
        }

        
        /// <summary>
        /// This will deactivate or activate the fly mode directly, the Property "Fly" will toggle the fly on and off... 
        /// this will set directly the fly on and off
        /// </summary>
        public virtual void SetFly(bool value)
        {
            if (canFly && hasFly)
            {
                fly = !value;
                Fly = true;
            }
        }


        /// <summary>
        /// This will enable the gravity while flying and add some drag
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetToGlide(float value)
        {
            if (fly && fall)
            {
                StartCoroutine(GravityDrag(value));
            }
        }

        internal IEnumerator GravityDrag(float value)
        {
            while (currentAnimState.tagHash != Hash.Fly) //Wait until the animal enters the Fly State
            {
                yield return null;
            }
            groundSpeed = 2;

            if (_RigidBody)
            {
                _RigidBody.useGravity = true;
                _RigidBody.drag = value;
            }
        }

        internal IEnumerator ToggleJump()
        {
            for (int i = 0; i < ToogleAmount; i++)
            {
                Jump = true;
                yield return null;
            }
            Jump = false;
        }

        internal IEnumerator ToggleAction()
        {
            for (int i = 0; i < ToogleAmount; i++)
            {            
                action = true;

                if (currentAnimState.tagHash == Hash.Action) break;
                yield return null;
            }
            action = false;     //Reset Action     

            if (!RealAnimatorState(Hash.Action)) ActionID = -1; //Means that it could not make an action animation
        }

        internal IEnumerator ToggleStun(float time)
        {
            Stun = true;
            yield return new WaitForSeconds(time);
            stun = false;
        }
    }
}
