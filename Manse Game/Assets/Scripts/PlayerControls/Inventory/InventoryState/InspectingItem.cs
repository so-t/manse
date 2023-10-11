using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.Inventory.InventoryState
{
    public class InspectingItem : InventoryState
    {
        private readonly Inventory _inventory;
        private readonly Vector3 _originPosition;

        public InspectingItem(Inventory inventory, Vector3 originPosition)
        {
            _inventory = inventory;
            _inventory.GetDisplayedObject().AddComponent<InspectionControls>();
            _originPosition = originPosition;
        }

        public override bool Exit()
        {
            _inventory.GetDisplayedObject().GetComponent<InspectionControls>().Destroy();
            _inventory.state = new ReturningToDisplay(_inventory, _originPosition);
            return true;
        }
    }
}