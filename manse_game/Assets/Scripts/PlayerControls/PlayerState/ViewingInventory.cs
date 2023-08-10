﻿using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class ViewingInventory : PlayerState
    {
        private Inventory.Inventory _inventory;
        
        public ViewingInventory(PlayerController playerController, Inventory.Inventory inventory)
        {
            Player = playerController;
            _inventory = inventory;
            _inventory.CreateDisplay();
        }

        public override void HandlePlayerInput()
        {
            if (Input.GetKeyDown("escape") || Input.GetKeyDown("i"))
            {
                _inventory.DestroyDisplay();
                Player.State = new Playing(Player);
            }
            else if (Input.GetAxisRaw("Horizontal") != 0.0f)
            {
                _inventory.Rotate(Input.GetAxisRaw("Horizontal"));
            }
        }
    }
}