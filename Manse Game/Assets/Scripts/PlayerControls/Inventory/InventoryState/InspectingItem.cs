using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.Inventory.InventoryState
{
    public class InspectingItem : InventoryState
    {
        private readonly Inventory _inventory;
        private readonly Vector3 _originPosition;
        private readonly Quaternion _originRotation;
        private readonly InspectionControls _controller;

        public InspectingItem(Inventory inventory, Vector3 originPosition)
        {
            _inventory = inventory;
            _originPosition = originPosition;
            
            var displayedObject = _inventory.GetDisplayedObject();
            _originRotation = displayedObject.transform.rotation;
            _controller = displayedObject.AddComponent<InspectionControls>();
        }

        public override bool Exit()
        {
            if (_controller)
                _controller.Destroy();
            
            _inventory.state = new ReturningToDisplay(_inventory, _originPosition, _originRotation);
            return true;
        }
    }
}