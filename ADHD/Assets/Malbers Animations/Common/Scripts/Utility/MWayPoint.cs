using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
    public class MWayPoint : MonoBehaviour
    {
        public static List<MWayPoint> WayPoints;

        public Transform NextWaypoint;
        public float StoppingDistance = 1;

        public bool debug = true;
        public bool dConnect = true;

        void OnEnable()
        {
            if (WayPoints == null)
            {
                WayPoints = new List<MWayPoint>();
            }

            WayPoints.Add(this);
        }

        void OnDisable()
        {
            WayPoints.Remove(this);
        }



#if UNITY_EDITOR
        /// <summary>
        /// DebugOptions
        /// </summary>
        void OnDrawGizmos()
        {
            if (debug)
            {
                Gizmos.color = Color.magenta;

                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, StoppingDistance);

                //UnityEditor.Handles.color = Color.white;
                //UnityEditor.Handles.Label(transform.position, name);


                if (NextWaypoint && dConnect)
                {
                    Debug.DrawLine(transform.position, NextWaypoint.transform.position, Color.green);
                }
            }
        }
#endif
    }
}
