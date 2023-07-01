using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using PlayerControls.Camera;
using PlayerControls.PlayerState;

namespace PlayerControls.Controller
{
    public class PlayerController : MonoBehaviour
    {   
        [FormerlySerializedAs("Speed")] public float speed = 10f;
        [FormerlySerializedAs("TurnSpeed")] public float turnSpeed = 125f;
        
        public PlayerState.PlayerState State;
        [FormerlySerializedAs("TextField")] public TMP_Text textField;
        [FormerlySerializedAs("CamRotation")] public CameraRotation camRotation;


        private void Awake()
        {
            State = new Playing(gameObject);
            textField = gameObject.GetComponentInChildren<TMP_Text>();
            camRotation = gameObject.GetComponentInChildren<CameraRotation>();
        }

        private bool IsPaused() 
        {
            return (State.GetType() == typeof(Paused));
        }

        public bool Interact(Transform lookAtTarget=null)
        {
            if (State.GetType() == typeof(Playing))
            {
                State = new Interacting(gameObject);
                if (lookAtTarget != null) camRotation.LookAt(lookAtTarget);
                return true;
            } 
            else return false;
        }

        private void ReturnToPlayState()
        {
            State = new Playing(gameObject);
        }

        private void Update()
        {
            State.HandlePlayerInput();

            if (Input.GetKeyDown("escape"))
            {
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
}