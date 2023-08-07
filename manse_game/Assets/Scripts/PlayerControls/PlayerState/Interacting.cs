using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class Interacting : PlayerState
    {
        public Interacting(PlayerController playerController)
        {
            Player = playerController;
        }
    }
}