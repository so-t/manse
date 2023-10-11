using PlayerControls.Camera;
using PlayerControls.Controller;
using TMPro;
using UI;
using UnityEngine;

namespace Interactables
{
    public class BaseInteractable : MonoBehaviour
    {
        public float cutoffDistance = 10f;
        public bool requiresInteraction = true;
        public bool requiresPlayerInRange = true;
        public bool requiresLineOfSight = true;
        public Transform lookTarget;
        public string displayText = "";
        
        protected bool fired;
        protected InteractableState state = InteractableState.Primed;
        protected PlayerController playerController;
        protected CameraRotation playerCamera;
        
        private UIUtilities _uiUtilities;

        private TMP_Text _tmp;
        private GameObject _player;

        protected enum InteractableState 
        {
            Primed,
            Triggered,
            Post,
            Finished
        }

        protected virtual void Awake()
        {
            _player = GameObject.Find("Player");
            _uiUtilities = GameObject.Find("UI").GetComponent<UIUtilities>();
            
            playerController = _player.GetComponentInChildren<PlayerController>();
            playerCamera = _player.GetComponentInChildren<CameraRotation>();
        }

        private bool PlayerInRange(float range)
        {
            var adjustedPosition = transform.position;
            var position = _player.transform.position;
            adjustedPosition.y = position.y;
            return Vector3.Distance(position, adjustedPosition) < range;
        }

        protected virtual bool StartCondition()
        {
            return !((requiresPlayerInRange && !PlayerInRange(cutoffDistance))
                     || (requiresInteraction && !Input.GetButtonDown("Interact"))
                     || (requiresLineOfSight && !playerController.CanSeeObject(gameObject)));
        }
        
        protected virtual void Action(){}

        protected bool MessageDisplayComplete()
        {
            return _uiUtilities.SubtitleTextMatches(displayText);
        }

        protected virtual bool ExitCondition() { return true; }
        
        protected virtual void Exit(){}

        protected virtual bool FireAction(){
            if (!fired && !playerCamera.hasTarget)
            {
                Action();
                _uiUtilities.EnableSubtitleDisplay();
                _uiUtilities.TeleTypeMessage(displayText);
                fired = true;
            }
            else if (ExitCondition())
            {
                Exit();
                _uiUtilities.DisableSubtitleDisplay();
                if (lookTarget) playerCamera.ReturnToLookTarget();
                fired = false;
                return true;
            }

            return false;
        }

        protected virtual bool FirePostAction() { return true; }

        private void Update()
        {
            switch (state)
            {
                case InteractableState.Primed:
                    if (StartCondition() && playerController.Interact(lookTarget))
                        state = InteractableState.Triggered;
                    break;
                case InteractableState.Triggered:
                    if (FireAction())
                        state = InteractableState.Post;
                    break;
                case InteractableState.Post when !playerCamera.hasTarget:
                    if (FirePostAction()) 
                    {
                        playerController.Resume();
                        state = InteractableState.Finished;
                    }
                    break;
                case InteractableState.Finished:
                    break;
            }
        }
    }
}
