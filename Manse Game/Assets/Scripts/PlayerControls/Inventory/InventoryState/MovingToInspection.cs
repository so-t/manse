using UnityEngine;

namespace PlayerControls.Inventory.InventoryState
{
    public class MovingToInspection : InventoryState
    {
        // TODO: Can this be made dynamic?
        private const float TargetOffsetZ = 0.39f;
        
        private readonly Inventory _inventory;
        private readonly GameObject _displayedObject;
        private readonly Vector3 _originPosition;
        private readonly Vector3 _targetPosition;
        private readonly float _movementRate;
        private readonly Rigidbody _rigidbody;
        private readonly float _rateCoefficient;
        private bool _controllerCreated;
        
        public MovingToInspection(Inventory inventory)
        {
            _inventory = inventory;
            _displayedObject = inventory.GetDisplayedObject();
            
            _rigidbody = _displayedObject.GetComponent<Rigidbody>();
            _displayedObject.transform.parent = null;

            _originPosition = _displayedObject.transform.position;
            
            _targetPosition = _inventory.GetDisplayCameraPosition();
            _targetPosition.z += TargetOffsetZ;

            _movementRate = Mathf.Max(
                Mathf.Abs(_targetPosition.y - _originPosition.y),
                Mathf.Abs(_targetPosition.z - _originPosition.z));

            var ls = _originPosition.z - _targetPosition.z;
            var rs = Mathf.Pow(_originPosition.y - _targetPosition.y, 2);
            _rateCoefficient = ls / rs;
        }

        public override void FixedUpdate()
        {
            var currentPosition = _displayedObject.transform.position;
            
            if (Mathf.Abs(currentPosition.z - _targetPosition.z) >= 0.001f)
            {
                // Calculate the objects target position
                var vertex = (_targetPosition.z, _targetPosition.y);
                var y = currentPosition.y + _movementRate * Time.fixedDeltaTime;
                var z = _rateCoefficient * Mathf.Pow(y - vertex.y, 2) + vertex.z;
                
                var pos = new Vector3(currentPosition.x, y, z);
                _rigidbody.position = pos;
            }
            else
            {
                _rigidbody.position = _targetPosition;
                _inventory.state = new InspectingItem(_inventory, _originPosition);
            }
        }
    }
}