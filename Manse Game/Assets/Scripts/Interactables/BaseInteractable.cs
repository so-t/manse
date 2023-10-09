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
        
        protected bool Fired;
        protected InteractableState State = InteractableState.Primed;
        protected PlayerController PlayerController;
        protected CameraRotation PlayerCamera;
        protected UIUtilities uiUtilities;

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
            uiUtilities = GameObject.Find("UI").GetComponent<UIUtilities>();
            
            PlayerController = _player.GetComponentInChildren<PlayerController>();
            PlayerCamera = _player.GetComponentInChildren<CameraRotation>();

            // if (!Application.isEditor) return;
            //
            // gameObject.transform.GetChild(0).localScale = 
            //     new Vector3(
            //         (gameObject.transform.GetChild(0).localScale.x / transform.GetChild(0).lossyScale.x) * cutoffDistance * 2, 
            //         (gameObject.transform.GetChild(0).localScale.y / transform.GetChild(0).lossyScale.y) * cutoffDistance * 2, 
            //         (gameObject.transform.GetChild(0).localScale.z / transform.GetChild(0).lossyScale.z) * cutoffDistance * 2
            //     );
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
                     || (requiresLineOfSight && !PlayerController.CanSeeObject(gameObject)));
        }
        
        protected virtual void Action(){}

        protected bool MessageDisplayComplete()
        {
            return _tmp.text == displayText;
        }

        protected virtual bool ExitCondition() { return true; }
        
        protected virtual void Exit(){}

        protected virtual bool FireAction(){
            if (!Fired && !PlayerCamera.hasTarget)
            {
                Action();
                var subtitleDisplay = uiUtilities.CreateSubtitleDisplay();
                _tmp = subtitleDisplay.gameObject.GetComponentInChildren<TMP_Text>();
                StartCoroutine(subtitleDisplay.TeleTypeMessage(displayText));
                Fired = true;
                Fired = true;
            }
            else if (ExitCondition())
            {
                Exit();
                uiUtilities.DestroySubtitleDisplay();
                if (lookTarget) PlayerCamera.ReturnToLookTarget();
                Fired = false;
                return true;
            }

            return false;
        }

        protected virtual bool FirePostAction() { return true; }

        private void Update()
        {
            switch (State)
            {
                case InteractableState.Primed:
                    if (StartCondition() && PlayerController.Interact(lookTarget))
                        State = InteractableState.Triggered;
                    break;
                case InteractableState.Triggered:
                    if (FireAction())
                        State = InteractableState.Post;
                    break;
                case InteractableState.Post when !PlayerCamera.hasTarget:
                    if (FirePostAction()) 
                    {
                        PlayerController.Resume();
                        State = InteractableState.Finished;
                    }
                    break;
                case InteractableState.Finished:
                    break;
            }
        }
    }
}
