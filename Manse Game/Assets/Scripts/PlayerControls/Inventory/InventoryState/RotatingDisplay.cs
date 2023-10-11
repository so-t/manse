namespace PlayerControls.Inventory.InventoryState
{
    public class RotatingDisplay : InventoryState
    {
        private readonly Inventory _inventory;
        
        public RotatingDisplay(Inventory inventory)
        {
            _inventory = inventory;

            if (_inventory.Count() == 1) 
                _inventory.state = new RotatingItem(_inventory);
        }

        public override void FixedUpdate()
        {
            if (_inventory.display.HasFinishedRotating())
            {
                _inventory.state = new RotatingItem(_inventory);
            }
            else
            {
                _inventory.display.Rotate();
            }
        }
    }
}