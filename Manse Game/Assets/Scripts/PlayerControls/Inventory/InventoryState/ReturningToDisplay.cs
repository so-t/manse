using UnityEngine;

namespace PlayerControls.Inventory.InventoryState
{
    public class ReturningToDisplay : InventoryState
    {
        private const float RotationSpeed = 180f;
        
        private readonly Inventory _inventory;
        private readonly Vector3 _targetPosition;
        private readonly Quaternion _targetRotation;
        private readonly Vector3 _rotationDirection;
        private readonly GameObject _displayedObject;
        private readonly Rigidbody _rigidbody;
        private readonly float _movementRate;
        private readonly float _rateCoefficient;

        public ReturningToDisplay(Inventory inventory, Vector3 targetPosition, Quaternion targetRotation)
        {
            _inventory = inventory;
            _targetPosition = targetPosition;
            _targetRotation = targetRotation;
            _displayedObject = inventory.GetDisplayedObject();
            
            _rigidbody = _displayedObject.GetComponent<Rigidbody>();
            _displayedObject.transform.parent = null;

            var originPosition = _displayedObject.transform.position;
            
            _movementRate = Mathf.Max(
                Mathf.Abs(_targetPosition.y - originPosition.y),
                Mathf.Abs(_targetPosition.z - originPosition.z));

            var ls = originPosition.y - _targetPosition.y;
            var rs = Mathf.Pow(originPosition.z - _targetPosition.z, 2);
            _rateCoefficient = ls / rs;
        }

        public override void FixedUpdate()
        {
            var currentPosition = _displayedObject.transform.position;
            var currentRotation = _displayedObject.transform.rotation;
            
            if (currentRotation != _targetRotation)
            {
                _displayedObject.transform.rotation = Quaternion.RotateTowards(
                        currentRotation, 
                        _targetRotation, 
                        RotationSpeed * Time.fixedDeltaTime);
            }
            else if (Mathf.Abs(currentPosition.z - _targetPosition.z) >= 0.001f)
            {
                // Calculate the objects target position
                var vertex = (_targetPosition.z, _targetPosition.y);
                var z = currentPosition.z + _movementRate * Time.fixedDeltaTime;
                var y = _rateCoefficient * Mathf.Pow(z - vertex.z, 2) + vertex.y;
                
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