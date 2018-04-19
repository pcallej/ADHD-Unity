using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Linq;
using System.Collections.Generic;
using System;

namespace MalbersAnimations
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Animal))]
    public class AnimalEditor : Editor
    {
        protected Animal myAnimal;
        MonoScript script;
        protected SerializedProperty
            animalTypeID, GroundLayer, StartSpeed, Height, WalkSpeed, TrotSpeed, RunSpeed,

            FallRayDistance, YAxisPositiveMultiplier, YAxisNegativeMultiplier,

            maxAngleSlope, SlowSlopes, forwardJumpControl, smoothJumpForward,

            GotoSleep, SnapToGround , AlingToGround, waterLine, swimSpeed, underWaterSpeed, bank,

            life, defense, attackStrength, FallRayMultiplier, debug, attackTotal, attackDelay, activeAttack, damageInterrupt,

            FlySpeed, upDownSmoothness, StartFlying, land;

        AnimatorController controller;
        List<AnimatorControllerParameter> parameters;
        private void OnEnable()
        {
            myAnimal = (Animal)target;
            script = MonoScript.FromMonoBehaviour(myAnimal);
            FindProperties();

            controller = (AnimatorController)myAnimal.Anim.runtimeAnimatorController;
            if (controller)  parameters = controller.parameters.ToList(); ///
        }

        bool FindParameter(int ParameterHash, AnimatorControllerParameterType Ptype)
        {
            if (parameters != null)
            {
                AnimatorControllerParameter founded = parameters.Find(item => item.nameHash == ParameterHash && item.type == Ptype);

                if (founded != null)   return true;
            }
            return false;
        }


        protected void FindProperties()
        {
            animalTypeID = serializedObject.FindProperty("animalTypeID");
            GroundLayer = serializedObject.FindProperty("GroundLayer");
            StartSpeed = serializedObject.FindProperty("StartSpeed");
            Height = serializedObject.FindProperty("height");

            WalkSpeed = serializedObject.FindProperty("walkSpeed");
            TrotSpeed = serializedObject.FindProperty("trotSpeed");
            RunSpeed = serializedObject.FindProperty("runSpeed");

            maxAngleSlope = serializedObject.FindProperty("maxAngleSlope");
            SlowSlopes = serializedObject.FindProperty("SlowSlopes");

            GotoSleep = serializedObject.FindProperty("GotoSleep");
            SnapToGround = serializedObject.FindProperty("SnapToGround");

            AlingToGround = serializedObject.FindProperty("AlingToGround");


            waterLine = serializedObject.FindProperty("waterLine");

            swimSpeed = serializedObject.FindProperty("swimSpeed");
            underWaterSpeed = serializedObject.FindProperty("underWaterSpeed");

            FlySpeed = serializedObject.FindProperty("flySpeed");
            StartFlying = serializedObject.FindProperty("StartFlying");
            land = serializedObject.FindProperty("land");

            life = serializedObject.FindProperty("life");
            defense = serializedObject.FindProperty("defense");
            attackStrength = serializedObject.FindProperty("attackStrength");
            attackDelay = serializedObject.FindProperty("attackDelay");

            attackTotal = serializedObject.FindProperty("TotalAttacks");
            activeAttack = serializedObject.FindProperty("activeAttack");
            damageInterrupt = serializedObject.FindProperty("damageInterrupt");

            FallRayDistance = serializedObject.FindProperty("FallRayDistance");
            FallRayMultiplier = serializedObject.FindProperty("FallRayMultiplier");

            forwardJumpControl = serializedObject.FindProperty("forwardJumpControl");
            smoothJumpForward = serializedObject.FindProperty("smoothJumpForward");


            upDownSmoothness = serializedObject.FindProperty("upDownSmoothness");
            YAxisNegativeMultiplier = serializedObject.FindProperty("YAxisNegativeMultiplier");
            YAxisPositiveMultiplier = serializedObject.FindProperty("YAxisPositiveMultiplier");

            debug = serializedObject.FindProperty("debug");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawAnimalInspector();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawAnimalInspector()
        {
            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Locomotion System", MessageType.None, true);
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            EditorGUI.BeginDisabledGroup(true);
            script = (MonoScript)EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();


            //────────────────────────────────── General ──────────────────────────────────

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            myAnimal.EditorGeneral = EditorGUILayout.Foldout(myAnimal.EditorGeneral, "General");
            EditorGUI.indentLevel--;

            if (myAnimal.EditorGeneral)
            {
                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);

                EditorGUILayout.PropertyField(GroundLayer, new GUIContent("Ground Layer", "Specify wich layer are Ground"));
                EditorGUILayout.PropertyField(StartSpeed, new GUIContent("Start Speed", "Activate the correct additive Animation to offset the Bones"));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(Height, new GUIContent("Height", "Distance from the ground"));
                if (GUILayout.Button( new GUIContent("C","Calculate the Height of the Animal"), EditorStyles.miniButton, GUILayout.Width(18)))
                {
                    if (!CalculateHeight())
                    {
                        EditorGUILayout.HelpBox("No pivots found, please add at least one Pivot (CHEST or HIP)", MessageType.Warning);
                    }
                } 
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                myAnimal.canSwim = GUILayout.Toggle( myAnimal.canSwim, new GUIContent("Can Swim", "Activate the Swim Logic\nif the Animator Controller of this animal does not have a 'Swim' Bool Parameter\nThis option should be disabled\nIt might cause unwanted behaviours"), EditorStyles.miniButton);
                myAnimal.canFly = GUILayout.Toggle( myAnimal.canFly, new GUIContent("Can Fly", "Activate the Fly Logic\nif the Animator Controller of this animal does not have a 'Fly' Bool Parameter\nThis option should be disabled\nIt might cause unwanted behaviours"), EditorStyles.miniButton);
                EditorGUILayout.EndHorizontal();

                if (myAnimal.canFly)
                {
                    if (!FindParameter(Hash.Fly, AnimatorControllerParameterType.Bool))
                    {
                        EditorGUILayout.HelpBox("The Animator Controller of this animal does not have a 'Fly' Bool Parameter\nThis option should be disabled\nIt might cause unwanted behaviours", MessageType.Warning);
                    }
                }

                if (myAnimal.canSwim)
                {
                    if (!FindParameter(Hash.Swim, AnimatorControllerParameterType.Bool))
                    {
                        EditorGUILayout.HelpBox("The Animator Controller of this animal does not have a 'Swim' Bool Parameter\nThis option should be disabled\nIt might cause unwanted behaviours", MessageType.Warning);
                    }
                }


                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

            //────────────────────────────────── Ground ──────────────────────────────────

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            myAnimal.EditorGround = EditorGUILayout.Foldout(myAnimal.EditorGround, "Ground");
            EditorGUI.indentLevel--;

            if (myAnimal.EditorGround)
            {
                DrawSpeed(WalkSpeed, "Walk");
                DrawSpeed(TrotSpeed, "Trot");
                DrawSpeed(RunSpeed, "Run");
            }

            EditorGUILayout.EndVertical();


            //────────────────────────────────── Water ──────────────────────────────────
            if (myAnimal.canSwim)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                myAnimal.EditorWater = EditorGUILayout.Foldout(myAnimal.EditorWater, "Swim");
                EditorGUI.indentLevel--;
                if (myAnimal.EditorWater)
                {
                    DrawSpeed(swimSpeed, "Swim");
                    EditorGUILayout.PropertyField(waterLine, new GUIContent("Water Line", "Aling the animal to the Water Surface"));
                    myAnimal.CanGoUnderWater = GUILayout.Toggle(myAnimal.CanGoUnderWater, new GUIContent("Can go Underwater", "Activate the UnderWater Logic"), EditorStyles.miniButton);

                    if (myAnimal.CanGoUnderWater)
                    {
                        if (myAnimal.CanGoUnderWater)
                        {
                            if (!FindParameter(Hash.Underwater, AnimatorControllerParameterType.Bool))
                            {
                                EditorGUILayout.HelpBox("The Animator Controller of this animal does not have a 'Underwater' Bool Parameter\nThis option should be disabled\nIt might cause unwanted behaviours", MessageType.Warning);
                            }
                        }

                        DrawSpeed(underWaterSpeed, "UnderWater");
                    }
                }
                EditorGUILayout.EndVertical();

            }

            //────────────────────────────────── AIR ──────────────────────────────────
            if (myAnimal.canFly)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                myAnimal.EditorAir = EditorGUILayout.Foldout(myAnimal.EditorAir, "Fly");
                EditorGUI.indentLevel--;
                if (myAnimal.EditorAir)
                {
                    DrawSpeed(FlySpeed, "Fly");

                    EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                    EditorGUILayout.PropertyField(StartFlying, new GUIContent("Start Flying", "Start in the FlyMode"));
                    EditorGUILayout.PropertyField(land, new GUIContent("Land", "When the animal is close to the Floor Disable the fly mode (LAND)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("LockUp"), new GUIContent("Lock Up", "The animal cannot fly upwards... just fly forward or down..."));
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }

            //────────────────────────────────── Atributes ──────────────────────────────────

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            myAnimal.EditorAttributes = EditorGUILayout.Foldout(myAnimal.EditorAttributes, "Attributes");
            EditorGUI.indentLevel--;

            if (myAnimal.EditorAttributes)
            {
                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                EditorGUILayout.PropertyField(life, new GUIContent("Life", "Life Points"));
                EditorGUILayout.PropertyField(defense, new GUIContent("Defense", "Defense Points"));
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(attackTotal, new GUIContent("Total Attacks", "Total of Animation Attacks"), GUILayout.MinWidth(25));
                EditorGUIUtility.labelWidth = 40;
                EditorGUILayout.PropertyField(activeAttack, new GUIContent("Active", "Currrent active Attack\n if value is -1 means it will play a random attack according the Total Attacks"), GUILayout.MinWidth(0));
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(attackStrength, new GUIContent("Attack Points", "Attack Hit Points"));
                EditorGUILayout.PropertyField(attackDelay, new GUIContent("Attack Delay", "Time for this animal to be able to Attack again. \nGreater number than the animation itself will be ignored"));

                if (myAnimal.attackDelay <= 0)
                {
                    EditorGUILayout.HelpBox("The Attack will not be interrupted if AttackDelay is below 0", MessageType.Info);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("damageDelay"), new GUIContent("Damage Delay", "Time which this animal can receive damage again (Immunity)"));
               // EditorGUILayout.PropertyField(damageInterrupt, new GUIContent("Damage Interrupt", "Time to move again after being hit"));
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("inmune"), new GUIContent("Inmune", "This animal cannot recieve damage"));
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndVertical();

            //────────────────────────────────── Air Control ──────────────────────────────────

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            myAnimal.EditorAirControl = EditorGUILayout.Foldout(myAnimal.EditorAirControl, "Jump/Fall Control");
            EditorGUI.indentLevel--;
            if (myAnimal.EditorAirControl)
            {
                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("airRotation"), new GUIContent("Jump/Fall Rotation", "Enables you to rotate the animal while jumping or falling"));
                EditorGUILayout.PropertyField(forwardJumpControl, new GUIContent("Jump Forward Stop", "Enables Stop moving forward while jumping"));
                if (myAnimal.forwardJumpControl)
                {
                    EditorGUILayout.PropertyField(smoothJumpForward, new GUIContent("Jump Control Smoothness", "Lerp between air stand and moving forward"));
                }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("JumpHeightMultiplier"), new GUIContent("Jump Height", "Adds More Height to the Jump. Check the JumpBehaviour on the Animator Controller"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("JumpForwardMultiplier"), new GUIContent("Jump Forward", "Adds More Height to the Jump. Check the JumpBehaviour on the Animator Controller"));

                EditorGUILayout.EndVertical();
            }
            //────────────────────────────────── Advanced ──────────────────────────────────
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            myAnimal.EditorAdvanced = EditorGUILayout.Foldout(myAnimal.EditorAdvanced, "Advanced");
            EditorGUI.indentLevel--;

            if (myAnimal.EditorAdvanced)
            {
                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                EditorGUILayout.PropertyField(animalTypeID, new GUIContent("Animal Type ID", "Enable the Additive Pose Animation to offset the Bones"));
                EditorGUILayout.PropertyField(GotoSleep, new GUIContent("Go to Sleep", "Number of Idles before going to sleep (AFK)"));

                EditorGUILayout.PropertyField(SnapToGround, new GUIContent("Snap to ground", "Smoothness to Snap to terrain"));
                EditorGUILayout.PropertyField(AlingToGround, new GUIContent("Align to ground", "Smoothness to aling to terrain"));

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                EditorGUILayout.PropertyField(maxAngleSlope, new GUIContent("Max Angle Slope", "Max Angle that the animal can walk"));
                EditorGUILayout.PropertyField(SlowSlopes, new GUIContent("Slow Slopes", "if the animal is going uphill: Slow it down"));
                EditorGUILayout.EndVertical();

               

                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                EditorGUILayout.PropertyField(FallRayDistance, new GUIContent("Front Fall Ray", "Multiplier to set the Fall Ray in front of the animal"));
                EditorGUILayout.PropertyField(FallRayMultiplier, new GUIContent("Fall Ray Multiplier", "Multiplier for the Fall Ray"));
                EditorGUILayout.EndVertical();
              

                EditorGUILayout.BeginHorizontal(MalbersEditor.FlatBox);
                EditorGUILayout.LabelField(new GUIContent("Locomotion Speed", "This are the values for the Animator Locomotion Blend Tree when the velocity is changed"), GUILayout.MaxWidth(120));
                myAnimal.movementS1 = EditorGUILayout.FloatField(myAnimal.movementS1, GUILayout.MinWidth(28));
                myAnimal.movementS2 = EditorGUILayout.FloatField(myAnimal.movementS2, GUILayout.MinWidth(28));
                myAnimal.movementS3 = EditorGUILayout.FloatField(myAnimal.movementS3, GUILayout.MinWidth(28));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("TurnMultiplier"), new GUIContent("Turn Multiplier", "When Using Directions or (CameraBasedInput) this will highly increase the rotation of the turn animations"));


                EditorGUILayout.PropertyField(upDownSmoothness, new GUIContent("Y Axis Smoothness", "Smoothness of the UPDOWN axis. when pressing 'Jump' to go UP or 'Down' to go Down"));
                EditorGUILayout.PropertyField(YAxisPositiveMultiplier, new GUIContent("+Y Axis Multiplier", "When Using Directions or (CameraBasedInput) for moving and the animal can Fly/SwimUnderWater this will increase the direction when going Upwards"));
                EditorGUILayout.PropertyField(YAxisNegativeMultiplier, new GUIContent("-Y Axis Multiplier", "When Using Directions or (CameraBasedInput) for moving and the animal can Fly/SwimUnderWater this will increase the direction when going Downwards"));
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("animatorSpeed"), new GUIContent("Animator Speed", "The global animator speed on the animal"));
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();


            //──────────────────────────────────────────────────── Events ────────────────────────────────────────────────────

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            myAnimal.EditorEvents = EditorGUILayout.Foldout(myAnimal.EditorEvents, "Events");
            EditorGUI.indentLevel--;

            if (myAnimal.EditorEvents)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnJump"), new GUIContent("On Jump"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnAttack"), new GUIContent("On Attack"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnGetDamaged"), new GUIContent("On Get Damaged"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnDeathE"), new GUIContent("On Death"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnAction"), new GUIContent("On Action"));
                if (myAnimal.canFly) EditorGUILayout.PropertyField(serializedObject.FindProperty("OnFly"), new GUIContent("On Fly"));
                if (myAnimal.canSwim) EditorGUILayout.PropertyField(serializedObject.FindProperty("OnSwim"), new GUIContent("On Swim"));
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(debug, new GUIContent("Debug"));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Animal Values Changed");
                EditorUtility.SetDirty(target);
            }
        }

        private bool CalculateHeight()
        {
            myAnimal.SetPivots();

            if (!myAnimal.Pivot_Hip && !myAnimal.Pivot_Chest) return false;

            Pivots pivot = myAnimal.Pivot_Hip ? myAnimal.Pivot_Hip : myAnimal.Pivot_Chest;

            Ray newHeight = new Ray()
            {
                origin = pivot.transform.position,
                direction = -Vector3.up * 5
            };

            RaycastHit hit;
            if (Physics.Raycast(newHeight, out hit, pivot.multiplier * myAnimal.ScaleFactor, myAnimal.GroundLayer))
            {
                myAnimal.height = hit.distance;
                serializedObject.ApplyModifiedProperties();
            }
            return false;
        }

        string position = "position";
        string AnimatorSpeed = "animator";
        string SmoothTS = "lerpPosition";
        string SmoothAS = "lerpAnimator";
        string turnSpeed = "rotation";
        string SmoothTurnSpeed = "lerpRotation";

        protected void DrawSpeed(SerializedProperty speed, string name)
        {
            float with = 48;
            EditorGUILayout.BeginVertical(MalbersEditor.FlatBox);
            EditorGUILayout.LabelField(name + " Speed", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(speed.FindPropertyRelative(position), new GUIContent("Position", "Additional " + name + " Speed added to the position"), GUILayout.MinWidth(with));
            EditorGUIUtility.labelWidth = 18;
            EditorGUILayout.PropertyField(speed.FindPropertyRelative(SmoothTS), new GUIContent("L", "Position " + name + " Lerp interpolation, higher value more Responsiveness"), GUILayout.MaxWidth(with));
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(speed.FindPropertyRelative(AnimatorSpeed), new GUIContent("Animator", "Animator " + name + " Speed"), GUILayout.MinWidth(with));
            EditorGUIUtility.labelWidth = 18;
            EditorGUILayout.PropertyField(speed.FindPropertyRelative(SmoothAS), new GUIContent("L", "Animator " + name + " Lerp interpolation, higher value more Responsiveness"), GUILayout.MaxWidth(with));
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(speed.FindPropertyRelative(turnSpeed), new GUIContent("Turn", "Aditional " + name + " Turn Speed"), GUILayout.MinWidth(with));
            EditorGUIUtility.labelWidth = 18;
            EditorGUILayout.PropertyField(speed.FindPropertyRelative(SmoothTurnSpeed), new GUIContent("L", "Rotation " + name + " Lerp interpolation, higher value more Responsiveness"), GUILayout.MaxWidth(with));
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}