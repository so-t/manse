using System;
using PlayerControls.Camera;
using PlayerControls.Controller;
using UI;
using UnityEngine;

namespace Interactables
{
    public class Interactable : MonoBehaviour
    {
        protected InteractableState State = InteractableState.Primed;
        protected TeleType TeleType;

        protected enum InteractableState 
        {
            Primed,
            Triggered,
            Post,
            Finished
        }
        protected bool Fired;

        public Transform cameraTarget;
        public bool requiresInteraction = true;

        public GameObject player;
        public PlayerController playerController;
        public CameraRotation playerCamera;
        public float cutoff = 10f;

        protected virtual void Awake()
        {
            player = GameObject.Find("Player");
            playerController = player.GetComponentInChildren<PlayerController>();
            playerCamera = player.GetComponentInChildren<CameraRotation>();
            
            if (!Application.isEditor) return;
            
            var localScale = gameObject.transform.localScale;
            gameObject.transform.GetChild(0).localScale = new Vector3(localScale.x + cutoff, 
                localScale.y + cutoff, 
                localScale.z + cutoff);
        }

        private bool PlayerInRange(float range)
        {
            Vector3 adjustedPosition = transform.position;
            var position = player.transform.position;
            adjustedPosition.y = position.y;
            return Vector3.Distance(position, adjustedPosition) < range;
        }

        protected virtual bool StartCondition()
        {
            return requiresInteraction switch
            {
                true when !Input.GetButtonDown("Interact") => false,
                _ => PlayerInRange(cutoff) && !playerCamera.hasTarget && Input.GetButtonDown("Interact")
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
                    if (StartCondition() && playerController.Interact(cameraTarget))
                        State = InteractableState.Triggered;
                    break;
                case InteractableState.Triggered:
                    if (FireAction())
                    {
                        if (cameraTarget) playerCamera.ReturnToLookTarget();
                        State = InteractableState.Post;
                    }
                    break;
                case InteractableState.Post when !playerCamera.hasTarget:
                    FirePostAction();
                    State = InteractableState.Finished;
                    break;
                case InteractableState.Finished:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
