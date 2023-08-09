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

        public override void HandlePlayerInput()
        {
            if (Input.GetKeyDown("escape"))
            {
                Player.Resume();
            }
        }
    }
}