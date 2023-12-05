using System;
using System.Globalization;
using UnityEngine;
using PlayerControls.Controller;
using PlayerControls.Controller.ControllerState;

namespace  PlayerControls.Camera
{
    public class CameraRotation : MonoBehaviour
    {
        private const float EventSpeed = 90f;
        
        private Vector2 _rotation = Vector2.zero;
        private Quaternion _targetRotation;
        private Transform _prevLookTarget;
        
        private Vector3 _lastMousePosition;
        private float _lastMouseInputTime;

        [Range(0f, 90f)] 
        public float rotationLimit = 88f;
        public float userSpeed = 5f;
        public float snapBackSpeed = 2f;
        public float snapBackDelay = 5f;
        public bool hasTarget;

        public PlayerController playerController;
        public GameObject player;

        public void Awake()
        {
            playerController = player.GetComponentInChildren<PlayerController>();
        }

        public void ReturnToLookTarget()
        {
            _targetRotation = _prevLookTarget.rotation;
            _prevLookTarget = null;
            hasTarget = true;
        }

        public void LookAt(Transform target=null)
        {
            if (!target) return;
            
            hasTarget = true;
            _prevLookTarget = transform;

            _targetRotation = Quaternion.LookRotation(target.position - _prevLookTarget.position);
        }
        
        private void SmoothLookAt()
        {
            transform.rotation = Quaternion.RotateTowards(
                 transform.rotation, _targetRotation, EventSpeed * Time.fixedDeltaTime);
             if(transform.rotation == _targetRotation) hasTarget = false;
        }

        private void Update()
        {
            // if (playerController.state.GetType() == typeof(Playing))
            // {
            //     if (Input.GetAxis("Camera Horizontal") != 0.0f || Input.GetAxis("Camera Vertical") != 0.0f)
            //     {
            //         _rotation.x += Input.GetAxis("Camera Horizontal") * userSpeed;
            //         _rotation.y += Input.GetAxis("Camera Vertical") * userSpeed;
            //         _rotation.x = Mathf.Clamp(_rotation.x, -rotationLimit, rotationLimit);
            //         _rotation.y = Mathf.Clamp(_rotation.y, -rotationLimit, rotationLimit);
            //     }
            //     else
            //     {
            //         _rotation.x += 
            //             Math.Abs(
            //                 Mathf.Sign(_rotation.x + -Mathf.Sign(_rotation.x) * EventSpeed) 
            //                 - Mathf.Sign(_rotation.x)) < 0.5f ? 
            //                         -Mathf.Sign(_rotation.x) * EventSpeed : 0.0f;
            //         _rotation.y += 
            //             Math.Abs(
            //                 Mathf.Sign(_rotation.y + -Mathf.Sign(_rotation.y) * EventSpeed) 
            //                      - Mathf.Sign(_rotation.y)) < 0.5f ? 
            //                         -Mathf.Sign(_rotation.y) * EventSpeed : 0.0f;
            //     }
            //     var xQuaternion = Quaternion.AngleAxis(_rotation.x, Vector3.up);
            //     var yQuaternion = Quaternion.AngleAxis(_rotation.y, Vector3.left);
            //     
            //     transform.localRotation = xQuaternion * yQuaternion;
            // }
        }

        private void FixedUpdate()
        {
            if (hasTarget) SmoothLookAt();

            else if (playerController.state.GetType() == typeof(Playing))
            {
                if (Input.mousePosition != _lastMousePosition)
                {
                    _lastMousePosition = Input.mousePosition;
                    _lastMouseInputTime = Time.fixedTime;
                    
                    _rotation.x += Input.GetAxis("Mouse X") * userSpeed;
                    _rotation.y += Input.GetAxis("Mouse Y") * userSpeed;
                    _rotation.x = Mathf.Clamp(_rotation.x, -rotationLimit, rotationLimit);
                    _rotation.y = Mathf.Clamp(_rotation.y, -rotationLimit, rotationLimit);

                    var xQuaternion = Quaternion.AngleAxis(_rotation.x, Vector3.up);
                    var yQuaternion = Quaternion.AngleAxis(_rotation.y, Vector3.left);
                    // TODO: Lerp
                    transform.localRotation = xQuaternion * yQuaternion;
                }
                else if (Time.fixedTime - _lastMouseInputTime >= snapBackDelay)
                {
                    // _rotation.x += 
                    //     Math.Abs(
                    //         Mathf.Sign(_rotation.x + -Mathf.Sign(_rotation.x) * snapBackSpeed) 
                    //         - Mathf.Sign(_rotation.x)) < 0.5f ? 
                    //                 -Mathf.Sign(_rotation.x) * snapBackSpeed : 0.0f;
                    _rotation.y += 
                        Math.Abs(
                            Mathf.Sign(_rotation.y + -Mathf.Sign(_rotation.y) * snapBackSpeed) 
                                 - Mathf.Sign(_rotation.y)) < 0.5f ? 
                                    -Mathf.Sign(_rotation.y) * snapBackSpeed : 0.0f;

                    var xQuaternion = Quaternion.AngleAxis(_rotation.x, Vector3.up);
                    var yQuaternion = Quaternion.AngleAxis(_rotation.y, Vector3.left);
                    transform.localRotation = xQuaternion * yQuaternion;
                }
            }
        }
    }
}