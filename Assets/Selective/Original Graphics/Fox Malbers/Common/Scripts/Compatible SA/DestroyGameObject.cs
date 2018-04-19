using UnityEngine;

namespace MalbersAnimations
{
    /// <summary>
    /// This is the same TimedObjectDestructor of the Stardard Assets
    /// </summary>
    public class DestroyGameObject : MonoBehaviour
    {
        [SerializeField] private float m_TimeOut = 1.0f;
        [SerializeField] private bool m_DetachChildren = false;


        private void Awake()
        {
            Invoke("DestroyNow", m_TimeOut);
        }


        private void DestroyNow()
        {
            if (m_DetachChildren)
            {
                transform.DetachChildren();
            }
            DestroyObject(gameObject);
        }
    }
}
