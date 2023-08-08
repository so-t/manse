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
        
        public bool requiresInteraction = true;
        public float cutoff = 10f;
        public Transform lookTarget;

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
            gameObject.transform.GetChild(0).localScale = new Vector3(localScale.x + cutoff, 
                localScale.y + cutoff, 
                localScale.z + cutoff);
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
                _ => PlayerInRange(cutoff) && !PlayerCamera.hasTarget && Input.GetButtonDown("Interact")
            };
        }
        
        protected virtual void Action(){}
        
        protected virtual bool ExitCondition(){ return false; }
        
        protected virtual void Exit(){}

        protected virtual bool FireAction(){
            if (!Fired)
            {
                Action();
                Fired = true;
            }
            else if (ExitCondition())
            {
                Exit();
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
