using PlayerControls.Camera;
using PlayerControls.Controller;
using UnityEngine;

namespace Interactables
{
    public class Interactable : MonoBehaviour
    {
        protected InteractableState State = InteractableState.Primed;

        protected enum InteractableState 
        {
            Primed,
            Triggered,
            Post,
            Finished
        }

        public GameObject player;
        public PlayerController playerController;
        public CameraRotation playerCamera;
        public TMPro.TextMeshProUGUI textField;
        public float cutoff = 10f;
    
        protected bool PlayerInRange(float range)
        {
            Vector3 adjustedPosition = transform.position;
            var position = player.transform.position;
            adjustedPosition.y = position.y;
            return Vector3.Distance(position, adjustedPosition) < range;
        }

        protected virtual void CheckTrigger(){}

        protected virtual void FireEvent(){}

        protected virtual void FirePostEvent(){}

        private void Start()
        {
            player = GameObject.Find("Player");
            textField = player.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            playerController = player.GetComponentInChildren<PlayerController>();
            playerCamera = player.GetComponentInChildren<CameraRotation>();
            
            if (!Application.isEditor) return;
            
            var localScale = gameObject.transform.localScale;
            gameObject.transform.GetChild(0).localScale = new Vector3(localScale.x + cutoff, 
                localScale.y + cutoff, 
                localScale.z + cutoff);
        }

        private void Update()
        {
            switch (State)
            {
                case InteractableState.Primed:
                    CheckTrigger();
                    break;
                case InteractableState.Triggered:
                    FireEvent();
                    break;
                case InteractableState.Post:
                    FirePostEvent();
                    break;
                case InteractableState.Finished:
                    break;
            }
        }
    }
}
