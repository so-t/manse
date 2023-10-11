using System;
using UnityEngine;
using PlayerControls.Camera;
using PlayerControls.Controller.ControllerState;
using UI;

namespace PlayerControls.Controller
{
    public class PlayerController : MonoBehaviour
    {   
        public float speed = 10f;
        public float turnSpeed = 125f;
        
        public ControllerState.ControllerState state;
        public CameraRotation camRotation;
        public AudioSource footstepAudioSource;
        public UIUtilities UIUtils;
        
        [NonSerialized]
        public Vector3 velocity;

        private Rigidbody _rigidbody;
        private Inventory.Inventory _inventory;

        private void Awake()
        {
            footstepAudioSource = GetComponent<AudioSource>();
            state = new Playing(this);
            camRotation = GetComponentInChildren<CameraRotation>();
            _rigidbody = GetComponent<Rigidbody>();
            _inventory = GetComponent<Inventory.Inventory>();
        }

        public void Pause()
        {
            state = new Paused(this);
            velocity = Vector3.zero;
        }

        public void Resume()
        {
            state = new Playing(this);
        }

        public void OpenInventory()
        {
            state = new ViewingInventory(this, _inventory);
            velocity = Vector3.zero;
        }

        public bool IsPaused()
        {
            return state.GetType() == typeof(Paused);
        }

        public bool IsMoving() => velocity != Vector3.zero;

        public bool CanSeeObject(GameObject obj)
        {
            var viewPosition = camRotation.gameObject.transform.position;
            var direction = (obj.transform.position - viewPosition).normalized;
            if (!Physics.Raycast(viewPosition, direction, out var hitInfo)) return false;
            
            return hitInfo.collider.gameObject.name == obj.name;
        }

        public bool Interact(Transform lookAtTarget=null)
        {
            if (state.GetType() != typeof(Playing)) return false;
            
            state = new Interacting(this);
            camRotation.LookAt(lookAtTarget);
            return true;
        }
        
        public void AddToInventory(GameObject obj)
        {
            _inventory.Add(obj);
        }

        public void RemoveFromInventory(GameObject obj)
        {
            _inventory.Remove(obj);
        }
        
        private void Update()
        {
            state.HandlePlayerInput();
        }

        private void FixedUpdate()
        {
            var t = transform;
            var rotation = velocity.y * turnSpeed * Time.fixedDeltaTime;
            
            transform.Rotate(t.up * rotation);
            _rigidbody.velocity = t.forward * (velocity.z * speed);
        }
    }
}