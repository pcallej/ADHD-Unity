using UnityEngine;
using System.Collections.Generic;
using System;
#if CROSS_PLATFORM_INPUT
using UnityStandardAssets.CrossPlatformInput;
#endif

namespace MalbersAnimations
{
    public enum InputType
    {
        Input, Key
    }

    public enum InputButton
    {
        Press, Down, Up
    }

    #region InputRow
    /// <summary>
    /// Input Class to change directly between Keys and Unity Inputs
    /// </summary>
    [Serializable]
    public class InputRow
    {
        public bool active = true;
        public string name = "Variable";
        public InputType type;
        public string input = "Value";
        public KeyCode key;
        public InputButton GetPressed;

        /// <summary>
        /// Return True or False to the Selected type of Input of choice
        /// </summary>
        public bool GetInput
        {
            get
            {
                if (!active) return false;
                switch (type)
                {
                    case InputType.Input:
                        switch (GetPressed)
                        {
                            case InputButton.Press:
#if !CROSS_PLATFORM_INPUT
                                return Input.GetButton(input);
#else
                                return  CrossPlatformInputManager.GetButton(input);
#endif
                            case InputButton.Down:
#if !CROSS_PLATFORM_INPUT
                                return Input.GetButtonDown(input);
#else
                                return  CrossPlatformInputManager.GetButtonDown(input);
#endif
                            case InputButton.Up:
#if !CROSS_PLATFORM_INPUT
                                return Input.GetButtonUp(input);
#else
                                return  CrossPlatformInputManager.GetButtonUp(input);
#endif
                        }
                        break;
                    case InputType.Key:
                        switch (GetPressed)
                        {
                            case InputButton.Press:
                                return Input.GetKey(key);
                            case InputButton.Down:
                                return Input.GetKeyDown(key);
                            case InputButton.Up:
                                return Input.GetKeyUp(key);
                        }
                        break;
                    default:
                        break;
                }
                return false;
            }
        }

        public void SetInputType(InputType type)
        {
            this.type = type;
        }

        #region Constructors
        public InputRow(string i)
        {
            active = true;
            type = InputType.Input;
            input = i;
            GetPressed = InputButton.Down;
        }

        public InputRow(KeyCode k)
        {
            active = true;
            type = InputType.Key;
            key = k;
            GetPressed = InputButton.Down;
        }

        public InputRow(string i, KeyCode k)
        {
            active = true;
            type = InputType.Key;
            key = k;
            input = i;
            GetPressed = InputButton.Down;
        }

        public InputRow(string unityInput, KeyCode k, InputButton pressed)
        {
            active = true;
            type = InputType.Key;
            key = k;
            input = unityInput;
            GetPressed = InputButton.Down;
        }

        public InputRow(string name, string unityInput, KeyCode k, InputButton pressed, InputType itype)
        {
            this.name = name;
            active = true;
            type = itype;
            key = k;
            input = unityInput;
            GetPressed = pressed;
        }

        #endregion
    }
    #endregion

    public class MalbersInput : MonoBehaviour
    {
        private iMalbersInputs character;
        private Vector3 m_CamForward;
        private Vector3 m_Move;
        private Transform m_Cam;
        public List<InputRow> inputs = new List<InputRow>();

        public bool ActiveHorizontal = true;
        public bool ActiveVertical = true;
        public string Horizontal = "Horizontal";
        public string Vertical = "Vertical";
        public bool AxisRaw = true;


        public bool cameraBaseInput;
        public bool alwaysForward;

        private float h;  //Horizontal Right & Left   Axis X
        private float v;  //Vertical   Forward & Back Axis Z

        #region Inputssss
        protected InputRow Attack1;
        protected InputRow Attack2;
        protected InputRow Action;
        protected InputRow Jump;
        protected InputRow Shift;
        protected InputRow Fly;
        protected InputRow Down;
        protected InputRow Dodge;
        protected InputRow Death;
        protected InputRow Stun;
        protected InputRow Damaged;
        protected InputRow Speed1;
        protected InputRow Speed2;
        protected InputRow Speed3;
        protected InputRow SpeedDown;
        protected InputRow SpeedUp;
        #endregion



        private void Reset()
        {
            inputs.Add(new InputRow("Jump", "Jump", KeyCode.Space, InputButton.Press, InputType.Input));
            inputs.Add(new InputRow("Shift", "Fire3", KeyCode.LeftShift, InputButton.Press, InputType.Input));
            inputs.Add(new InputRow("Attack1", "Fire1", KeyCode.Mouse0, InputButton.Press, InputType.Input));
            inputs.Add(new InputRow("Attack2", "Fire2", KeyCode.Mouse1, InputButton.Press, InputType.Input));
            inputs.Add(new InputRow("Speed1", "Speed1", KeyCode.Alpha1, InputButton.Down, InputType.Key));
            inputs.Add(new InputRow("Speed2", "Speed2", KeyCode.Alpha2, InputButton.Down, InputType.Key));
            inputs.Add(new InputRow("Speed3", "Speed3", KeyCode.Alpha3, InputButton.Down, InputType.Key));

            InputRow speedUP = new InputRow("SpeedUp", "SpeedUp", KeyCode.Alpha2, InputButton.Down, InputType.Key)
            { active = false };

            InputRow speedDown = new InputRow("SpeedDown", "SpeedDown", KeyCode.Alpha1, InputButton.Down, InputType.Key)
            { active = false };

            inputs.Add(speedDown);
            inputs.Add(speedUP);
        
            inputs.Add(new InputRow("Action", "Action", KeyCode.E, InputButton.Down, InputType.Key));
            inputs.Add(new InputRow("Fly", "Fly", KeyCode.Q, InputButton.Down, InputType.Key));
            inputs.Add(new InputRow("Dodge", "Dodge", KeyCode.R, InputButton.Down, InputType.Key));
            inputs.Add(new InputRow("Down", "Down", KeyCode.C, InputButton.Press, InputType.Key));
            inputs.Add(new InputRow("Stun", "Stun", KeyCode.T, InputButton.Press, InputType.Key));

            inputs.Add(new InputRow("Death", "Death", KeyCode.K, InputButton.Down, InputType.Key));
            inputs.Add(new InputRow("Damaged", "Damaged", KeyCode.H, InputButton.Down, InputType.Key));
        }

        public bool CameraBaseInput
        {
            get { return cameraBaseInput; }
            set { cameraBaseInput = value; }
        }

        void Awake()
        {
            character = GetComponent<iMalbersInputs>();  //get the animal Script
            FindAllInputs();
        }

        private void FindAllInputs()
        {
            Attack1 = FindInput("Attack1");
            Attack2 = FindInput("Attack2");
            Action = FindInput("Action");
            Jump = FindInput("Jump");
            Shift = FindInput("Shift");
            Fly = FindInput("Fly");
            Down = FindInput("Down");
            Dodge = FindInput("Dodge");
            Death = FindInput("Death");
            Stun = FindInput("Stun");
            Damaged = FindInput("Damaged");
            Speed1 = FindInput("Speed1");
            Speed2 = FindInput("Speed2");
            Speed3 = FindInput("Speed3");

            SpeedUp = FindInput("SpeedUp");
            SpeedDown = FindInput("SpeedDown");

        }

        private void Start()
        {
            if (Camera.main != null)   // get the transform of the main camera
                m_Cam = Camera.main.transform;
        }

        void OnDisable()
        {
            if (character != null) character.MovementAxis = Vector3.zero;       //When the Input is Disable make sure the character/animal is not moving.
        }

        // Fixed update is called in sync with physics
        void Update()
        {
            if (AxisRaw)
            {

           
#if !CROSS_PLATFORM_INPUT
            h = Input.GetAxisRaw(Horizontal);
            v = alwaysForward ? 1 : Input.GetAxisRaw(Vertical);
#else
            h = CrossPlatformInputManager.GetAxisRaw(Horizontal);
            v = alwaysForward ? 1 : CrossPlatformInputManager.GetAxisRaw(Vertical);
#endif
            }
            else
            {
#if !CROSS_PLATFORM_INPUT
                h = Input.GetAxis(Horizontal);
                v = alwaysForward ? 1 : Input.GetAxis(Vertical);
#else
            h = CrossPlatformInputManager.GetAxis(Horizontal);
            v = alwaysForward ? 1 : CrossPlatformInputManager.GetAxis(Vertical);
#endif
            }
            if (!ActiveVertical) v = 0;
            if (!ActiveHorizontal) h = 0;
            if (character != null) SetInput();

        }

        public virtual Vector3 CameraInputBased()
        {
            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, Vector3.one).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
            return m_Move;
        }

        /// <summary>
        /// Send all the Inputs to the Animal
        /// </summary>
        protected virtual void SetInput()
        {
            if (cameraBaseInput)
            {
                character.Move(CameraInputBased());
            }
            else
            {
                character.Move(new Vector3(h, 0, v), false);
            }

            if (Attack1 != null && Attack1.active) character.Attack1 = Attack1.GetInput;        //Get the Attack1 button
            if (Attack2 != null && Attack2.active) character.Attack2 = Attack2.GetInput;        //Get the Attack2 button

            if (Action != null && Action.active) character.Action = Action.GetInput;            //Get the Action/Emotion button

            if (Jump != null && Jump.active) character.Jump = Jump.GetInput;                    //Get the Jump button
            if (Shift != null && Shift.active) character.Shift = Shift.GetInput;                //Get the Shift button

            if (Fly != null && Fly.active) character.Fly = Fly.GetInput;                        //Get the Fly button
            if (Down != null && Down.active) character.Down = Down.GetInput;                    //Get the Down button
            if (Dodge != null && Dodge.active) character.Dodge = Dodge.GetInput;                //Get the Dodge button

            if (Stun != null && Stun.active) character.Stun = Stun.GetInput;                    //Get the Stun button
            if (Death != null && Death.active) character.Death = Death.GetInput;                //Get the Death button
            if (Damaged != null && Damaged.active) character.Damaged = Damaged.GetInput;        //Get the Damaged button

            if (Speed1 != null && Speed1.active) character.Speed1 = Speed1.GetInput;            //Get the Speed1 button
            if (Speed2 != null && Speed2.active) character.Speed2 = Speed2.GetInput;            //Get the Speed2 button
            if (Speed3 != null && Speed3.active) character.Speed3 = Speed3.GetInput;            //Get the Speed3 button

            if (SpeedUp != null && SpeedUp.active) character.SpeedUp = SpeedUp.GetInput;            //Get the Speed3 button
            if (SpeedDown != null && SpeedDown.active) character.SpeedDown = SpeedDown.GetInput;            //Get the Speed3 button

        }

        /// <summary>
        /// Enable/Disable the Input
        /// </summary>
        public virtual void EnableInput(string inputName, bool value)
        {
            InputRow i = inputs.Find(item => item.name == inputName);

            if (i != null) i.active = value;
        }

        /// <summary>
        /// Thit will set the correct Input, from the Unity Input Manager or Keyboard.. you can always modify this code
        /// </summary>
        protected bool GetInput(string name)
        {
            InputRow input = inputs.Find(item => item.name.ToUpper() == name.ToUpper());

            if (input != null) return input.GetInput;

            return false;
        }

        /// <summary>
        /// Check if the input is active
        /// </summary>
        public virtual bool IsActive(string name)
        {
            InputRow input = inputs.Find(item => item.name.ToUpper() == name.ToUpper());

            if (input != null) return input.active;

            return false;
        }

        public virtual InputRow FindInput(string name)
        {
            InputRow input = inputs.Find(item => item.name.ToUpper() == name.ToUpper());

            if (input != null) return input;

            return null;
        }
    }
}