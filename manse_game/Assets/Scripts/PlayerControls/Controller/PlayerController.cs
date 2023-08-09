using Items;
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
        
        private Rigidbody _rigidbody;
        private Vector3 _velocity;
        private Inventory.Inventory _inventory = new Inventory.Inventory();
        private TeleType _teleType;

        private void Awake()
        {
            footstepAudioSource = GetComponent<AudioSource>();
            State = new Playing(this);
            textField = GetComponentInChildren<TMP_Text>();
            camRotation = GetComponentInChildren<CameraRotation>();
            _rigidbody = GetComponent<Rigidbody>();
            _teleType = GetComponentInChildren<TeleType>();
        }

        private bool IsPaused()
        {
            return State.GetType() == typeof(Paused);
        }

        public bool Interact(Transform lookAtTarget=null)
        {
            if (State.GetType() != typeof(Playing)) return false;
            
            State = new Interacting(this);
            camRotation.LookAt(lookAtTarget);
            return true;
        }

        public void ReturnToPlayState()
        {
            State = new Playing(this);
        }

        public bool CameraHasTarget()
        {
            return camRotation.hasTarget;
        }

        public void AddToInventory(Item item)
        {
            _inventory.Add(item);
        }

        public void RemoveFromInventory(Item item)
        {
            _inventory.Remove(item);
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
            _velocity = State.HandlePlayerInput();

            // TODO: Move this to individual PlayerStates rather than putting it here
            if (!Input.GetKeyDown("escape")) return;
            if (State.GetType() == typeof(Playing))
            {
                State = new Paused(this);
                Time.timeScale = 0;
            }
            else if (IsPaused())
            {
                ReturnToPlayState();
                Time.timeScale = 1;
            }
        }

        private void FixedUpdate()
        {
            var t = transform;
            var rotation = _velocity.y * turnSpeed * Time.fixedDeltaTime;
            var forward = t.forward * (_velocity.z * speed * Time.fixedDeltaTime);
            transform.Rotate(t.up * rotation);
            _rigidbody.MovePosition(_rigidbody.position + forward);
        }
    }
}