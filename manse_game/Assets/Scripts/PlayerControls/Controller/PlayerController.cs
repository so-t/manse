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
        [NonSerialized]
        public Vector3 Velocity;

        private Rigidbody _rigidbody;
        private Inventory.Inventory _inventory;
        private TeleType _teleType;

        private void Awake()
        {
            footstepAudioSource = GetComponent<AudioSource>();
            State = new Playing(this);
            textField = GetComponentInChildren<TMP_Text>();
            camRotation = GetComponentInChildren<CameraRotation>();
            _rigidbody = GetComponent<Rigidbody>();
            _inventory = GetComponent<Inventory.Inventory>();
            _teleType = GetComponentInChildren<TeleType>();
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
            StartCoroutine(_teleType.DisplayMessage(str));
        }
        
        public void ClearMessage()
        {
            _teleType.Clear();
        }
        
        private void Update()
        {
            State.HandlePlayerInput();
        }

        private void FixedUpdate()
        {
            var t = transform;
            var rotation = Velocity.y * turnSpeed * Time.fixedDeltaTime;
            var velocity = Velocity.z * speed * Time.fixedDeltaTime;
            transform.Rotate(t.up * rotation);
            _rigidbody.MovePosition(_rigidbody.position + t.forward * velocity);
        }
    }
}