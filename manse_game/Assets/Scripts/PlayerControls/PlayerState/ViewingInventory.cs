using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class ViewingInventory : PlayerState
    {
        public ViewingInventory(PlayerController playerController)
        {
            Player = playerController;
        }

        public override void HandlePlayerInput()
        {
            if (Input.GetKeyDown("escape") || Input.GetKeyDown("i"))
            {
                Player.CloseInventory();
            }
        }
    }
}