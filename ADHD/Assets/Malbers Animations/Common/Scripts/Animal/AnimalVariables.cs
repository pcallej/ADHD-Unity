using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using MalbersAnimations.Events;
using MalbersAnimations.Utilities;


namespace MalbersAnimations
{
    /// <summary>
    /// Saves the direction and the Amount of damage
    /// </summary>
    public class DamageValues
    {
        public Vector3 Direction;
        public float Amount = 0;

        public DamageValues(Vector3 dir, float amount = 0)
        {
            Direction = dir;
            Amount = amount;
        }
    }

    [System.Serializable]
    public class Speeds
    {
        /// <summary>
        /// Add additional speed to the transfrom
        /// </summary>
        public float position = 0;

        /// <summary>
        /// Changes the Animator Speed
        /// </summary>
        public float animator = 1;

        /// <summary>
        /// Smoothness to change to the Transform speed, higher value more Responsiveness
        /// </summary>
        public float lerpPosition = 2f;

        /// <summary>
        /// Smoothness to change to the Animator speed, higher value more Responsiveness
        /// </summary>
        public float lerpAnimator = 2f;

        /// <summary>
        /// Add Aditional Rotation to the Speed
        /// </summary>
        public float rotation = 0;

        public float lerpRotation = 2f; 

        public Speeds()
        {
            position = 0;
            animator = 1;
            lerpPosition = 4f;
            lerpAnimator = 4f;
            rotation = 0;
            lerpRotation = 4f;
        }

        public Speeds(float lerpPos, float lerpanim, float lerpTurn)
        {
            position = 0;
            animator = 1;
            rotation = 0;
            lerpPosition = lerpPos;
            lerpAnimator = lerpanim;
            lerpRotation = lerpTurn;
        }

    }

    [System.Serializable]
    public class AnimalEvent : UnityEvent<Animal> {}

    
    public partial class Animal
    {
        public enum Ground { walk = 1, trot = 2, run = 3 }

        /// <summary>
        /// List of all the animals on the scene
        /// </summary>
        public static List<Animal> Animals;

        #region Components 
        protected Animator anim;                    //Reference for the Animator
        protected Rigidbody _rigidbody;             //Reference for the RigidBody
        private Renderer animalMesh;                //Reference for the Mesh Renderer of this animal
        #endregion

        #region Animator Variables
        protected Vector3 movementAxis;             //In this variable will store Forward (Z) and horizontal (X) Movement

        protected bool
            speed1,                     //Walk (Set by Input)
            speed2,                     //Trot (Set by Input)
            speed3,                     //Run  (Set by Input)
            movementReleased = false,   

            jump,                       //Jump (Set by Input)
            fly,                        //Fly  (Set by Input)
            shift,                      //Sprint or Speed Swap (Set by Input)
            down,                       //Crouch, fly Down or Swim Underwater (Set by Input)
            dodge,                      //Dodge (Set by Input)
            fall, fallback,             //If is falling, (Automatic: Fall method)
            
            isInWater,                  //if is Entering Water(Trigger by WaterEnter Script or if the RayWater Hit the WaterLayer)
            isInAir,                    //if is Jumping or falling (Automatic: Fix Position Method)
            swim,                       //if is in Deep Water (Automatic: Swim method)
            underwater,                 //While Swimming Go underwater 
           
            stun,                       //Stunned (Set by Input or callit by a property)
            action,                     //Actions (Set by Input combined with Action ID)  
            stand = true,               //If Horizontal and Vertical are =0 (Automatic)
            backray,                    //Check if the Back feet are touching the ground 
            frontray;                   //Check if the Front feet are touching the ground

        /// <summary>
        /// Value for the Vertical Parameter on the Animator (Calculated from the Movement.z multiplied by the Speeds (Walk Trot Run) and Speed.Lerps
        /// </summary>
        protected float vertical;

        protected float horizontal,             //Direction from the Horizontal input multiplied by the Shift for turning 180    
           groundSpeed = 1f,                   //Current Ground Speed (Walk=1, Trot=2, Run =3) 
           slope,                              //Normalized Angle Slope from the MAX Angle Slope 
           idfloat,                            //Float values for the animator
           _Height;                            //Height from the ground to the hip 

        protected int
            idInt,                  //Integer values for the animator      
            actionID = -1,          //Every Actions has an ID this combined with the action bool will activate an action animation
            tired,                  //Counter to go to SleepMode (AFK) 
            loops = 1;              //Some aniamtions can have multiple loops like the wing attack from the dragon or eat on the animals
        #endregion

        public int animalTypeID;    //This parameter exist to Add Additive pose to the animal

        private Vector3 deltaPosition = Vector3.zero;               //All the Position Modifications 
        private Quaternion deltaRotation = Quaternion.identity;     //All the Rotation Modifications 

        #region JumpControl
        public float JumpHeightMultiplier = 0;
        public float JumpForwardMultiplier = 0;

        #endregion

        #region Ground
        public LayerMask GroundLayer = 1;
        public Ground StartSpeed = Ground.walk;

        public float height = 1f;                   //Distance from the Pivots to the ground

        public Speeds walkSpeed = new Speeds(8,4,6);
        public Speeds trotSpeed = new Speeds(4,4,6);
        public Speeds runSpeed = new Speeds(2,4,6);

        protected float CurrentAnimatorSpeed = 1; 

        protected Transform platform;
        protected Vector3 platform_Pos;
        protected float platform_formAngle;


        #region AirControl
        public float airRotation = 100;
        public bool forwardJumpControl = false;
        public float smoothJumpForward = 2;
        #endregion


        public float movementS1 = 1, movementS2 = 2, movementS3 = 3;        //IMPORTANT this are the values for the Animator Locomotion Blend Tree when the velocity is changed (Ex. Horse has 5 velocities)

        /// <summary>
        /// Maximun angle on the terrain the animal can walk
        /// </summary>
        [Range(0f, 90f)]
        public float maxAngleSlope  =  45f;
        public bool SlowSlopes = true;

        [Range(0, 100)]
        public int GotoSleep;

        public float SnapToGround = 20f;
        public float AlingToGround = 10f;
        public float FallRayDistance = 0.1f;
        public float FallRayMultiplier = 1f;

        public float TurnMultiplier = 100;
        public float YAxisPositiveMultiplier = 1;
        public float YAxisNegativeMultiplier = 1;


        #region Water Variables
        public float waterLine = 0f;                    //WaterLine
        public Speeds swimSpeed = new Speeds();         //SwimSpeed
        internal int WaterLayer;                        //Water Layer ID
        public bool canSwim = true;
        private float waterlevel = 0;                   //Temporal Water Level value
        public bool CanGoUnderWater;

        [Range(0,90)]
        public float bank = 0;
        public Speeds underWaterSpeed = new Speeds();         //SwimSpeed
       //float UnderWaterShift = 1;                           //Shift  
        #endregion

        #region Fly Variables
        public Speeds flySpeed = new Speeds();                  //Fly Speed Values
        /// <summary>
        /// On Start the nimal will be set to fly
        /// </summary>
        public bool StartFlying;                                //Set to fly at Start
        /// <summary>
        /// Can the animal fly?
        /// </summary>
        public bool canFly;                                     //is the Fly Logic Active?
        /// <summary>
        /// When the animal is near to the ground it will land automatically
        /// </summary>
        public bool land = true;                                //if Land true means that when is close to the ground it will exit the Fly State
        protected float LastGroundSpeed;                        //To save the las ground speed before it start flying
        
        /// <summary>
        /// The animal cannot fly upwards... just fly forward or down...
        /// </summary>
        public bool LockUp = false;                             //
        #endregion


        #region Attributes Variables (Attack, Damage)
        public float life = 100;
        public float defense = 0;
        public float damageDelay = 0.5f;            //Time before can aply damagage again
        public float damageInterrupt = 0.2f;        //NOT IMPLEMENTED YET

        public int TotalAttacks = 3;
        public int activeAttack = -1;
        public float attackStrength = 10;
        public float attackDelay = 0.5f;

        public bool inmune;
       
        protected bool 
            attack1, attack2,                   //Attacks (Set by Input) 
            isAttacking,                        //Set to true whenever an Attack Animations is played (Set by Animator) 
            isTakingDamage,                     //Prevent to take damage while this variable is true
            damaged,                            //GetHit (Set by OnTriggerEnter)
            death;                              //Death (Set by Life<0)

        protected List<AttackTrigger> Attack_Triggers;      //List of all the Damage Triggers on this Animal.
        #endregion
        #endregion

        public float animatorSpeed = 1f;
        public float upDownSmoothness = 2f;
        public bool debug = true;

        //------------------------------------------------------------------------------
        #region Modify_the_Position_Variables
        protected RaycastHit hit_Hip, hit_Chest; //Hip and Chest Ray Cast Information
        protected Vector3 
            fall_Point, 
            _hitDirection, 
            UpVector = Vector3.up;


        protected float scaleFactor = 1;
            

        protected List<Pivots> pivots = new List<Pivots>();
        protected Pivots pivot_Chest, pivot_Hip, pivot_Water;

        protected Vector3 surfaceNormal;        

      
        #endregion

        #region Optional Animator Parameters Activation
        bool hasFly;
        bool hasDodge;
        bool hasSlope;
        bool hasStun;
        bool hasAttack2;
        bool hasUpDown;
        bool hasUnderwater;
        bool hasSwim;
        #endregion

        #region Events
        public UnityEvent OnJump;
        public UnityEvent OnAttack;
        public FloatEvent OnGetDamaged;
        public UnityEvent OnDeathE;
        public UnityEvent OnAction;
        public UnityEvent OnSwim;
        public BoolEvent OnFly;
        public UnityEvent OnUnderWater;

        [HideInInspector] public UnityEvent OnSyncAnimator;     //Used for Sync Animators
        #endregion

        #region Properties
        /// <summary>
        /// Get the RigidBody
        /// </summary>
        public Rigidbody _RigidBody
        {
            get
            {
                if (_rigidbody == null)
                {
                    _rigidbody = GetComponentInChildren<Rigidbody>();
                }
                return _rigidbody;
            }
        }

        /// <summary>
        /// Current Animal Speed in numbers, 1 = walk 2 = trot 3 = run 
        /// </summary>
        public float GroundSpeed
        {
            set { groundSpeed = value; }
            get { return groundSpeed; }
        }


        /// <summary>
        /// Speed from the Vertical input multiplied by the speeds inputs(Walk Trot Run) this is the value thats goes to the Animator, is not the actual Speed of the animals
        /// </summary>
        public float Speed
        {
            set { vertical = value; }
            get { return vertical; }
        }

        /// <summary>
        /// Normalize Slope of the terrain
        /// </summary>
        public float Slope
        {
            get { return slope; }
        } 
        
        /// <summary>
        /// Checking if the input for forward or turn changed. (IntID = -2 for Released) (IntID = 0 for Not Released)
        /// </summary>
        public bool MovementReleased
        {
            private set
            {
                if (movementReleased != value)
                {
                    movementReleased = value;

                    if (!RealAnimatorState(Hash.Action) && !Action) SetIntID(value ? -2 : 0); //Send to the animator that the Movement was released by setting it to (-2) or to (0) if it wasn't;
                }
            }
            get { return movementReleased; }
        }

       

        /// <summary>
        /// Used to only change the swim from false or true every 0.5 sec to avoid intermitent changes
        /// </summary>
        public bool Swim
        {
            private set
            {
                if (swim != value && Time.time - swimChanged >= 0.5f)      //All of this is for not changing back to false or true immidiatly; Wait 0.5 sec...
                {
                    swim = value;
                    swimChanged = Time.time;
                }
            }
            get { return swim; }
        }
        float swimChanged;

        /// <summary>
        /// Direction from the Horizontal input multiplied by the speeds inputs(Walk Trot Run) this is the value thats goes to the Animator, is not the actual Speed of the animals
        /// </summary>
        public float Direction
        {
            get { return horizontal; }
        }

        /// <summary>
        /// Controls the Loops for some animations that can be played for an ammount of cycles.
        /// </summary>
        public int Loops
        {
            set { loops = value; }
            get { return loops; }
        }

        public int IDInt
        {
            set { idInt = value; }
            get { return idInt; }
        }

        public float IDFloat
        {
            set { idfloat = value; }
            get { return idfloat; }
        }

        /// <summary>
        /// Amount of Idle acumulated if the animals is not moving, if Tired is greater than GotoSleep the animal will go to the sleep state.
        /// </summary>
        public int Tired
        {
            set { tired = value; }
            get { return tired; }
        }

        /// <summary>
        /// Is the animal on water? not necessarily swimming
        /// </summary>
        public bool IsInWater
        {
            get { return isInWater; }
        }

        /// <summary>
        /// Change the Speed Up
        /// </summary>
        public bool SpeedUp
        {
            set
            {
                if (value)
                {
                    if (groundSpeed == movementS1) Speed2 = true;
                    else if (groundSpeed == movementS2) Speed3 = true;
                }
            }
        }

        /// <summary>
        /// Changes the Speed Down
        /// </summary>
        public bool SpeedDown
        {
            set
            {
                if (value)
                {
                    if (groundSpeed == movementS3) Speed2 = true;
                    else if (groundSpeed == movementS2) Speed1 = true;
                }
            }
        }


       protected float V_Smooth = 2;    //Smooth value for the Vertical Movement
       protected float H_Smooth = 8;    //Smooth value for the Horizontal Movement

        /// <summary>
        /// Set the Animal Speed to Speed1
        /// </summary>
        public bool Speed1
        {
            get { return speed1; }

            set
            {
                if (value)
                {
                    speed1 = value;
                    speed2 = speed3 = false;
                    groundSpeed = movementS1;

                    V_Smooth = walkSpeed.lerpPosition;
                    H_Smooth = walkSpeed.lerpRotation;
                }
            }
        }
      
        /// <summary>
        /// Set the Animal Speed to Speed2
        /// </summary>
        public bool Speed2
        {
            get { return speed2; }

            set
            {
                if (value)
                {
                    speed2 = value;
                    speed1 = speed3 = false;
                    groundSpeed = movementS2;

                    V_Smooth = trotSpeed.lerpPosition;
                    H_Smooth = trotSpeed.lerpRotation;
                }
            }
        }

        /// <summary>
        /// Set the Animal Speed to Speed3
        /// </summary>
        public bool Speed3
        {
            get { return speed3; }
            set
            {
                if (value)
                {
                    speed3 = value;
                    speed2 = speed1 = false;
                    groundSpeed = movementS3;

                    V_Smooth = runSpeed.lerpPosition;
                    H_Smooth = runSpeed.lerpRotation;
                }
            }
        }

        public bool Jump
        {
            get { return jump; }
            set { jump = value; }
        }

        /// <summary>
        /// is the Animal UnderWater
        /// </summary>
        public bool Underwater
        {
            get { return underwater; }
            set
            {
                if (CanGoUnderWater) underwater = value; //Just change the Underwater Variable if can GoUnderWater is enabled
            }
        }

        public bool Shift
        {
            get { return shift; }
            set { shift = value; }
        }

        public bool Down
        {
            get { return down; }
            set { down = value; }
        }

        public bool Dodge
        {
            get { return dodge; }
            set { dodge = value; }
        }

        public bool Damaged
        {
            get { return damaged; }
            set { damaged = value; }
        }

        /// <summary>
        /// Toogle the Fly on and Off!!
        /// </summary>
        public bool Fly
        {
            get
            {
                if (!canFly) fly = false;                       //Set back the fly to dalse if canfly is false
                return fly;
            }
            set
            {
                if (!canFly) { return; }                        //Do nothing if canfly is false

                if (value)                                      //Only When true
                {
                    fly = !fly;                                 //Toogle Fly

                    if (fly)                                    //OnFly Enabled!
                    {
                        if (_RigidBody) _RigidBody.useGravity = false;          //Deactivate gravity in case use gravity is off
                        LastGroundSpeed = groundSpeed;
                        groundSpeed = 1;                        //Change velocity to 1
                        IsInAir = true;

                        Quaternion finalRot = Quaternion.FromToRotation(transform.up, UpVector) * transform.rotation;
                        StartCoroutine(MalbersTools.AlignTransformsC(transform, finalRot, 0.3f));    //Quick Align the Fly
                    }
                    else
                    {
                        groundSpeed = LastGroundSpeed;          //Restore the Ground Speed
                    }
                    OnFly.Invoke(fly);                          //Invoke the Event OnFly;
                }
            }
        }

        /// <summary>
        /// If set to true the animal will die
        /// </summary>
        public bool Death
        {
            get { return death; }
            set
            {
                death = value;
                if (death)
                {
                    Anim.SetTrigger(Hash.Death);                           //Triggers the death animation.

                    Anim.SetBool(Hash.Attack1, false);                     //Reset the Attack1 on the animator
                    if (hasAttack2) Anim.SetBool(Hash.Attack2, false);     //Reset the Attack2 on the animator
                
                    Anim.SetBool(Hash.Action , false);                     //Reset the Action on the animator

                    OnDeathE.Invoke();                                     //Invoke the animal is death
                    if (Animals.Count > 0) Animals.Remove(this);           //Remove this animal of the animal list because is dead
                }
            }
        }

        /// <summary>
        /// Enables the Attack to the Current Active Attack
        /// </summary>
        public bool Attack1
        {
            get { return attack1; }
            set
            {
                if (!value) attack1 = value;

                if (death) return;                              //Don't Attack while your death 
                if (RealAnimatorState(Hash.Action)) return;     //Don't Attack when is making an action

                if (!isAttacking)                                       //Attack when is not attacking
                {
                    if (value)                                          //If Attack was set to true
                    {
                        attack1 = value;
                        IDInt = activeAttack;                           //Change the IntID to the Active attack ID

                        if (IDInt <= 0) SetIntIDRandom(TotalAttacks);   // if the Active Attack == -1 then Play Random Attacks
                        OnAttack.Invoke();
                    }
                }
            }
        }

        public bool Attack2
        {
            get { return attack2; }
            set
            {
                if (death) return;                                                      //If im death dont attack L:)

                if (value)                                                              //If Attack was set to true
                {
                    if (RealAnimatorState(Hash.Action)) return;                         //Don't Attack when is making an action
                }
                attack2 = value;
            }
        }

        public bool Stun
        {
            get { return stun; }
            set { stun = value; }
        }

        public bool Action
        {
            get { return action; }
            set
            {
                if (ActionID == -1) return;                         //There's no Action Active
                if (death) return;                                  //if you're death no not play any action.    

                //if (RealAnimatorState(Hash.Action))
                //{
                //    Anim.SetFloat(Hash.IDFloat, IDFloat = value ? 1 : 0);   //To know if the ID FLOAT IS PRESSED
                //    return;                                                 //Don't Make an New action if you already are making an action
                //}

                if (action != value)
                {
                    action = value;

                    if (action)
                    {
                        OnAction.Invoke();                          //Invoke on Action
                        StartCoroutine(ToggleAction());
                    }
                }
            }
        }

        public int ActionID
        {
            get { return actionID; }
            set { actionID = value; }
        }

        public bool IsAttacking
        {
            get { return isAttacking; }
            set { isAttacking = value; }
        }

        /// <summary>
        /// Change the Animator rootMotion value
        /// </summary>
        public bool RootMotion
        {
            set { Anim.applyRootMotion = value; }
            get { return Anim.applyRootMotion; }
        }

       

        /// <summary>
        /// Is the Animal on the Air, modifies the rigidbody constraints depending the IsInAir Value
        /// </summary>
        public bool IsInAir
        {
            get { return isInAir; }
            set
            {
                if (isInAir != value)
                {
                    isInAir = value;
                    if (isInAir) StillConstraints(!IsInAir);
                }
            }
        }

        public bool Stand
        {
            get { return stand; }
        }

        public Vector3 HitDirection
        {
            get { return _hitDirection; }
            set { _hitDirection = value; }
        }
        
        /// <summary>
        /// The Scale Factor of the Animal.. if the animal has being scaled this is the multiplier for the raycasting things
        /// </summary>
        public float ScaleFactor
        {
            get { return scaleFactor; }
        }

        public Pivots Pivot_Hip
        {
            get { return pivot_Hip; }
        }

        public Pivots Pivot_Chest
        {
            get { return pivot_Chest; }
        }

        AnimatorStateInfo currentAnimState;
        AnimatorStateInfo nextAnimState;


        /// <summary>
        /// Returns the Current Animation State of animal
        /// </summary>
        public AnimatorStateInfo CurrentAnimState
        {
            get { return currentAnimState; }
        }

        /// <summary>
        /// Returns the Current Animation State of animal
        /// </summary>
        public AnimatorStateInfo NextAnimState
        {
            get { return nextAnimState; }
        }

        /// <summary>
        /// Returns the Animator Component of the Animal
        /// </summary>
        public Animator Anim
        {
            get
            {
                if (anim == null)
                {
                    anim = GetComponent<Animator>();
                }
                return anim;
            }
        }

        public Vector3 Pivot_fall
        {
            get {return fall_Point;}
        }

        public float Chest_Pivot_Multiplier
        {
            get { return pivot_Chest ? pivot_Chest.multiplier * scaleFactor : 1 * scaleFactor; }
        }

        public Vector3 Chest_Pivot_Point
        {
            get
            {
                if (pivot_Chest) return pivot_Chest.GetPivot;
                if (pivot_Hip) return pivot_Hip.GetPivot;

                Vector3 Chest = transform.position;
                Chest.y += height;
                return Chest;
            }
        }


        /// <summary>
        /// Locks Position Y and AllRotations on the Rigid Body
        /// </summary>
        public static RigidbodyConstraints Still_Constraints
        {
            get { return RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY; }
        }

        /// <summary>
        /// Direction the animal to move
        /// </summary>
        public Vector3 MovementAxis
        {  
            get { return movementAxis; }
            set { movementAxis = value; }
        }


        public float MovementForward
        {
            get { return movementAxis.z; }
            set { movementAxis.z = value;
                MovementReleased = value == 0;
            }
        }

        public float MovementRight
        {
            get { return movementAxis.x; }
            set { movementAxis.x = value;
                MovementReleased = value == 0;
            }
        }

        public float MovementUp
        {
            get { return movementAxis.y; }
            set { movementAxis.y = value;
                MovementReleased = value == 0;
            }
        }

        /// <summary>
        /// The Normal vector resulted from the Hip and Chest Pivots and the terrain
        /// </summary>
        public Vector3 SurfaceNormal
        {
            get { return surfaceNormal; }
        }

        public Vector3 DeltaPosition
        {
            get { return deltaPosition; }
            set { deltaPosition = value; }
        }
        public Quaternion DeltaRotation
        {
            get { return deltaRotation; }
            set { deltaRotation = value; }
        }

        public Renderer AnimalMesh
        {
            get
            {
                return animalMesh;
            }

            set
            {
                animalMesh = value;
            }
        }

        #endregion

        #region UnityEditor Variables
        [HideInInspector] public bool EditorGeneral = true;
        [HideInInspector] public bool EditorGround = true;
        [HideInInspector] public bool EditorWater = true;
        [HideInInspector] public bool EditorAir = true;
        [HideInInspector] public bool EditorAdvanced = true;
        [HideInInspector] public bool EditorAirControl = true;
        [HideInInspector] public bool EditorAttributes = true;
        [HideInInspector] public bool EditorEvents = true;
        #endregion
    }
}
