using UI;

namespace PlayerControls.Inventory.InventoryState
{
    public class RotatingItem : InventoryState
    {
        private readonly Inventory _inventory;
        
        public RotatingItem(Inventory inventory)
        {
            _inventory = inventory;
            _inventory.display.StopRotating();
            _inventory.DisplaySelectedObjectName();
        }

        public override bool InspectObject()
        {
            _inventory.state = new MovingToInspection(_inventory);
            return true;
        }

        public override void RotateDisplay(float direction)
        {
            if (_inventory.display == null || direction == 0.0f) return;
            
            _inventory.ClearDisplayText();
            _inventory.display.SetRotationDirection(direction);
            _inventory.displayedItemIndex = direction switch
            {
                > 0.0f => _inventory.displayedItemIndex - 1 < 0 ? _inventory.Count() - 1 : _inventory.displayedItemIndex - 1,
                < 0.0f => _inventory.displayedItemIndex + 1 >= _inventory.Count() ? 0 : _inventory.displayedItemIndex + 1,
                _ => _inventory.displayedItemIndex
            };

            _inventory.state = new RotatingDisplay(_inventory);
        }

        public override void FixedUpdate()
        {
            InventoryDisplay.RotateDisplayObject(_inventory.GetDisplayedObject());
        }
    }
}