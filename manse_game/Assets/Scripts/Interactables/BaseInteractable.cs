using PlayerControls.Camera;
using PlayerControls.Controller;
using UI;
using UnityEngine;

namespace Interactables
{
    public class BaseInteractable : MonoBehaviour
    {
        private GameObject _player;
        
        protected bool Fired;
        protected InteractableState State = InteractableState.Primed;
        protected TeleType TeleType;
        protected PlayerController PlayerController;
        protected CameraRotation PlayerCamera;
        
        public float cutoffDistance = 10f;
        public bool requiresInteraction = true;
        public Transform lookTarget;
        public string displayText = "";

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
            PlayerController = _player.GetComponentInChildren<PlayerController>();
            PlayerCamera = _player.GetComponentInChildren<CameraRotation>();
            
            if (!Application.isEditor) return;
            
            var localScale = gameObject.transform.localScale;
            gameObject.transform.GetChild(0).localScale = new Vector3(
                localScale.x + cutoffDistance, 
                localScale.y + cutoffDistance, 
                localScale.z + cutoffDistance
                );
        }

        private bool PlayerInRange(float range)
        {
            Vector3 adjustedPosition = transform.position;
            var position = _player.transform.position;
            adjustedPosition.y = position.y;
            return Vector3.Distance(position, adjustedPosition) < range;
        }

        protected virtual bool StartCondition()
        {
            return requiresInteraction switch
            {
                true when !Input.GetButtonDown("Interact") => false,
                _ => PlayerInRange(cutoffDistance) && !PlayerCamera.hasTarget && Input.GetButtonDown("Interact")
            };
        }
        
        protected virtual void Action(){}
        
        protected virtual bool ExitCondition(){ return false; }
        
        protected virtual void Exit(){}

        protected virtual bool FireAction(){
            if (!Fired)
            {
                Action();
                if (displayText != "") TeleType = PlayerController.CreateTeleType(displayText);
                Fired = true;
            }
            else if (ExitCondition())
            {
                Exit();
                if (displayText != "") TeleType.Clear();
                Fired = false;
                return true;
            }

            return false;
        }

        protected virtual void FirePostAction(){}

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
                    {
                        if (lookTarget) PlayerCamera.ReturnToLookTarget();
                        State = InteractableState.Post;
                    }
                    break;
                case InteractableState.Post when !PlayerCamera.hasTarget:
                    FirePostAction();
                    State = InteractableState.Finished;
                    break;
                case InteractableState.Finished:
                    break;
            }
        }
    }
}
