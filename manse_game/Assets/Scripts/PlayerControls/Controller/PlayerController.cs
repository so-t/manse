using System;
using UnityEngine;
using TMPro;
using PlayerControls.Camera;
using PlayerControls.PlayerState;
using UI;

namespace PlayerControls.Controller
{
    public class PlayerController : MonoBehaviour
    {   
        public float speed = 10f;
        public float turnSpeed = 125f;
        
        public PlayerState.PlayerState State;
        public TMP_Text textField;
        public CameraRotation camRotation;
        public AudioSource footstepAudioSource;
        public SubtitleDisplay subtitleDisplay;
        [NonSerialized]
        public Vector3 Velocity;

        private Rigidbody _rigidbody;
        private Inventory.Inventory _inventory;

        private void Awake()
        {
            footstepAudioSource = GetComponent<AudioSource>();
            State = new Playing(this);
            camRotation = GetComponentInChildren<CameraRotation>();
            _rigidbody = GetComponent<Rigidbody>();
            _inventory = GetComponent<Inventory.Inventory>();
        }

        public void Pause()
        {
            State = new Paused(this);
            Velocity = Vector3.zero;
        }

        public void Resume()
        {
            State = new Playing(this);
        }

        public void OpenInventory()
        {
            State = new ViewingInventory(this, _inventory);
            Velocity = Vector3.zero;
        }

        public bool IsPaused()
        {
            return State.GetType() == typeof(Paused);
        }

        public bool IsMoving() => Velocity != Vector3.zero;

        public bool CanSeeObject(GameObject obj)
        {
            var viewPosition = camRotation.gameObject.transform.position;
            var direction = (obj.transform.position - viewPosition).normalized;
            if (!Physics.Raycast(viewPosition, direction, out var hitInfo)) return false;
            
            return hitInfo.collider.gameObject.name == obj.name;
        }

        public bool Interact(Transform lookAtTarget=null)
        {
            if (State.GetType() != typeof(Playing)) return false;
            
            State = new Interacting(this);
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
        
        public void DisplayMessage(string str)
        {
            if (str == "") return;
            StartCoroutine(subtitleDisplay.TeleTypeMessage(str));
        }
        
        public void ClearMessage()
        {
            subtitleDisplay.Disable();
        }
        
        private void Update()
        {
            State.HandlePlayerInput();
        }

        private void FixedUpdate()
        {
            var t = transform;
            var rotation = Velocity.y * turnSpeed * Time.fixedDeltaTime;
            var velocity = Velocity.z * speed;
            transform.Rotate(t.up * rotation);
            _rigidbody.velocity = t.forward * velocity;
        }
    }
}