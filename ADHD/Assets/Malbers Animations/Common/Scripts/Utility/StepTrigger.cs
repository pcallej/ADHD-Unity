using UnityEngine;
using System.Collections;


namespace MalbersAnimations
{
    
    public class StepTrigger : MonoBehaviour
    {
        StepsManager _StepsManager;

        public float WaitNextStep = 0.2f;

        [HideInInspector]
        public AudioSource StepAudio;

        bool hastrack;                      // Check if already  has a track... don't put another
        bool waitrack;                      // Check if is time to put a track; 
        public bool HasTrack
        {
            get { return hastrack; }
            set { hastrack = value; }
        }

        void Awake()
        {
            _StepsManager = GetComponentInParent<StepsManager>();

            StepAudio = GetComponent<AudioSource>();

            if (StepAudio == null)
            {
                StepAudio = gameObject.AddComponent<AudioSource>();
            }

            StepAudio.spatialBlend = 1;  //Make the Sound 3D
            if (_StepsManager) StepAudio.volume = _StepsManager.StepsVolume;
        }


        void OnTriggerEnter(Collider other)
        {
            if (!waitrack && _StepsManager)             //
            {
                 StartCoroutine(WaitForStep(WaitNextStep));     //Wait Half a Second before making another Step

                _StepsManager.EnterStep(this);
                hastrack = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            hastrack = false; // if the feet is on the air then can put a track
        }

        IEnumerator WaitForStep(float seconds)
        {
            waitrack =  true;
            yield return new WaitForSeconds(seconds);
            waitrack = false;
        }
    }
}