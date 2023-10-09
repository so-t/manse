using System;
using UnityEngine;
using PlayerControls.PlayerState;
using PlayerControls.Controller;

namespace  PlayerControls.Camera
{
    public class CameraRotation : MonoBehaviour
    {
        private Vector2 _rotation = Vector2.zero;

        [Range(0f, 90f)] 
        public float rotationLimit = 88f;

        public GameObject player;
        
	    public float userSpeed = 2f;
        public float eventSpeed = 0.5f;

        public bool hasTarget;
        private Vector3 _targetPosition;
        private Vector3 _prevLookTarget;

        public PlayerController playerController;

        public void Awake()
        {
            playerController = player.GetComponentInChildren<PlayerController>();
        }

        public void ReturnToLookTarget()
        {
            // TODO: Need to slow this down.
            // Currently Returning to the forward before LookAt was prompted jerks the camera very fast.
            hasTarget = true;
            _targetPosition = _prevLookTarget;
        }

        public void LookAt(Transform target=null)
        {
            if (!target) return;
            
            hasTarget = true;
            _targetPosition = target.position;
            var transform1 = transform;
            _prevLookTarget = transform1.position + transform1.forward;
        }
        
        private void SmoothLookAt(Vector3 target)
        {
            var transformCopy = transform;
            Vector3 forward = transformCopy.forward;
            var toRotation = Quaternion.LookRotation(target - transformCopy.position);
            transform.rotation = Quaternion.RotateTowards(
                 transform.rotation, toRotation, eventSpeed * Time.time);
             if(forward == transform.forward) hasTarget = false;
        }

        private void Update()
        {
            if (playerController.State.GetType() == typeof(Playing))
            {
                if (Input.GetAxis("Camera Horizontal") != 0.0f || Input.GetAxis("Camera Vertical") != 0.0f)
                {
                    _rotation.x += Input.GetAxis("Camera Horizontal") * userSpeed;
                    _rotation.y += Input.GetAxis("Camera Vertical") * userSpeed;
                    _rotation.x = Mathf.Clamp(_rotation.x, -rotationLimit, rotationLimit);
                    _rotation.y = Mathf.Clamp(_rotation.y, -rotationLimit, rotationLimit);
                }
                else
                {
                    _rotation.x += 
                        Math.Abs(
                            Mathf.Sign(_rotation.x + -Mathf.Sign(_rotation.x) * eventSpeed) 
                            - Mathf.Sign(_rotation.x)) < 0.5f ? 
                                    -Mathf.Sign(_rotation.x) * eventSpeed : 0.0f;
                    _rotation.y += 
                        Math.Abs(
                            Mathf.Sign(_rotation.y + -Mathf.Sign(_rotation.y) * eventSpeed) 
                                 - Mathf.Sign(_rotation.y)) < 0.5f ? 
                                    -Mathf.Sign(_rotation.y) * eventSpeed : 0.0f;
                }
                var xQuaternion = Quaternion.AngleAxis(_rotation.x, Vector3.up);
                var yQuaternion = Quaternion.AngleAxis(_rotation.y, Vector3.left);
                
                transform.localRotation = xQuaternion * yQuaternion;
            }
        }

        private void FixedUpdate()
        {
            if (hasTarget) SmoothLookAt(_targetPosition);
        }
    }
}