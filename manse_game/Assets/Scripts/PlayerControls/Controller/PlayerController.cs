using UnityEngine;
using TMPro;
using PlayerControls.Camera;
using PlayerControls.PlayerState;

namespace PlayerControls.Controller
{
    public class PlayerController : MonoBehaviour
    {   
        public float speed = 10f;
        public float turnSpeed = 125f;
        
        public PlayerState.PlayerState State;
        public TMP_Text textField;
        public CameraRotation camRotation;

        private Inventory.Inventory _inventory;

        private void Awake()
        {
            State = new Playing(gameObject);
            _inventory = new Inventory.Inventory();
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
            
            State = new Interacting(gameObject);
            if (lookAtTarget != null) camRotation.LookAt(lookAtTarget);
            return true;
        }

        private void ReturnToPlayState()
        {
            State = new Playing(gameObject);
        }

        private void Update()
        {
            State.HandlePlayerInput();

            // TODO: Move this to individual PlayerStates rather than putting it here
            if (!Input.GetKeyDown("escape")) return;
            if (State.GetType() == typeof(Playing))
            {
                State = new Paused(gameObject);
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