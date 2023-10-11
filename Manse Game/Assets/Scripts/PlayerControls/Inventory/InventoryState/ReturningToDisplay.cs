using PlayerControls.Controller;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerControls.Inventory.InventoryState
{
    public class ReturningToDisplay : InventoryState
    {
        private readonly Inventory _inventory;
        private readonly Vector3 _targetPosition;
        private readonly Vector3 _originPosition;
        private readonly GameObject _displayedObject;
        private readonly Rigidbody _rigidbody;
        private readonly float _movementRate;
        private readonly float _a;
        
        public ReturningToDisplay(Inventory inventory, Vector3 originPosition)
        {
            _inventory = inventory;
            _targetPosition = originPosition;
            
            _inventory = inventory;
            
            _displayedObject = inventory.GetDisplayedObject();
            _rigidbody = _displayedObject.GetComponent<Rigidbody>();

            _displayedObject.transform.parent = null;

            _originPosition = _displayedObject.transform.position;
            
            var distance = Mathf.Max(
                Mathf.Abs(_targetPosition.y - _originPosition.y),
                Mathf.Abs(_targetPosition.z - _originPosition.z));
            _movementRate = distance;

            var ls = _originPosition.y - _targetPosition.y;
            var rs = Mathf.Pow(_originPosition.z - _targetPosition.z, 2);
            _a = ls / rs;
        }

        public override void FixedUpdate()
        {
            var currentPosition = _displayedObject.transform.position;
            
            if (Mathf.Abs(currentPosition.z - _targetPosition.z) >= 0.001f)
            {
                // Calculate the objects target position
                var vertex = (_targetPosition.z, _targetPosition.y);
                var z = currentPosition.z + _movementRate * Time.deltaTime;
                var y = _a * Mathf.Pow(z - vertex.z, 2) + vertex.y;
                
                var pos = new Vector3(currentPosition.x, y, z);
                _rigidbody.position = pos;
            }
            else
            {
                _rigidbody.position = _targetPosition;
                _displayedObject.transform.SetParent(GameObject.Find("Inventory Display").transform);
                _inventory.state = new RotatingItem(_inventory);
            }
        }
    }
}