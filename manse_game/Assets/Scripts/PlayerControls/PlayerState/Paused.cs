using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class Paused : PlayerState
    {
        public Paused(PlayerController playerController)
        {
            Player = playerController;
        }
    }
}