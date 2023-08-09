using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class PlayerState
    {
        protected PlayerController Player;

        public virtual Vector3 HandlePlayerInput(){ return Vector3.zero; }
    }
}