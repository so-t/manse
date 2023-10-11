using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class InspectingItem : PlayerState
    {
        private Inventory.Inventory _inventory;

        public InspectingItem(PlayerController playerController, Inventory.Inventory inventory)
        {
            Player = playerController;
            _inventory = inventory;
        }
        
        private void ReturnToInventoryDisplay()
        {
            if (_inventory.Exit())
                Player.state = new ViewingInventory(Player, _inventory, false);
        }

        public override void HandlePlayerInput()
        {
            if (Input.GetKeyDown("escape"))
            {
                ReturnToInventoryDisplay();
            }
        }
    }
}
