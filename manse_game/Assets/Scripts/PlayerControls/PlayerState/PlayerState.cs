using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class PlayerState
    {
        protected Vector3 Velocity;
        protected PlayerController Player;

        public virtual void HandlePlayerInput(){}
    }
}