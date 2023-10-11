namespace PlayerControls.Inventory.InventoryState
{
    public class InventoryState
    {
        public virtual bool InspectObject() { return false; }

        public virtual bool Exit() { return false; }

        public virtual void RotateDisplay(float direction){}
        
        public virtual void Update(){}
        
        public virtual void FixedUpdate(){}
    }
}