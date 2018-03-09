using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    [CreateAssetMenu(menuName = "Malbers Animations/Actions")]
    public class Actions : ScriptableObject
    {
        [SerializeField]
        public ActionsEmotions[] actions;
    }
}