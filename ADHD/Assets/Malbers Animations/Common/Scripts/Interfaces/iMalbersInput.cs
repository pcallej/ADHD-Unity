using UnityEngine;

namespace MalbersAnimations
{
    /// <summary>
    /// Basic Entries needed to use Malbers Input Component
    /// </summary>
    public interface iMalbersInputs
    {
        bool Speed1 { set; }
        bool Speed2 { set; }
        bool Speed3 { set; }
        bool SpeedUp { set; }
        bool SpeedDown { set; }



        bool Jump { get; set; }
        bool Shift { get; set; }
        bool Down { get; set; }

        bool Dodge { get; set; }
        bool Damaged { get; set; }
        bool Fly { get; set; }

        bool Death { get; set; }

        bool Attack1 { get; set; }
        bool Attack2 { get; set; }
        bool Stun { get; set; }
        bool Action { get; set; }

        Vector3 MovementAxis { get; set; }

        void Move(Vector3 move, bool active = true);
    }
}