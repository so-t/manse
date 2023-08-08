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

        private Inventory.Inventory _inventory = new Inventory.Inventory();

        private void Awake()
        {
            footstepAudioSource = gameObject.GetComponent<AudioSource>();
            State = new Playing(this);
            textField = gameObject.GetComponentInChildren<TMP_Text>();
            camRotation = gameObject.GetComponentInChildren<CameraRotation>();
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

        private void ReturnToPlayState()
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
        
        public TeleType CreateTeleType(string str)
        {
            TeleType t = textField.gameObject.AddComponent<TeleType>();
            t.enabled = false;
            t.str = str;
            t.textMeshPro = textField;
            t.enabled = true;
            return t;
        }

        private void Update()
        {
            State.HandlePlayerInput();

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
    }
}