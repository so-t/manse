using PlayerControls.Controller;
using PlayerControls.Inventory.InventoryState;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class ViewingInventory : PlayerState
    {
        private readonly Inventory.Inventory _inventory;
        
        public ViewingInventory(PlayerController playerController, 
            Inventory.Inventory inventory, bool createDisplay=true)
        {
            Player = playerController;
            _inventory = inventory;
            
            if (createDisplay)
            {
                _inventory.CreateDisplay();
                _inventory.state = new RotatingItem(_inventory);
            }
        }

        public override void HandlePlayerInput()
        {
            if (Input.GetKeyDown("escape") || Input.GetKeyDown("i"))
            {
                _inventory.DestroyDisplay();
                Player.state = new Playing(Player);
            }
            else if (Input.GetAxisRaw("Horizontal") != 0.0f)
            {
                _inventory.RotateDisplay(Input.GetAxisRaw("Horizontal"));
            }
            else if (Input.GetButtonDown("Interact"))
            {
                if (_inventory.InspectObject());
                    Player.state = new InspectingItem(Player, _inventory);
            }
        }
    }
}