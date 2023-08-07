using Events;
using PlayerControls.PlayerState;
using UnityEngine;

namespace Interactables
{
    public class EventListener : Interactable
    {
        private bool _fired;

        [SerializeField]
        private EventOutcome _eventOutcome;

        protected override void CheckTrigger()
        {
            if (PlayerInRange(cutoff) 
                && Input.GetButtonDown("Interact") 
                && playerController.Interact(transform)
                && !_fired)
                State = InteractableState.Triggered;
        }

        protected override void FireEvent() 
        {
            if (!_fired)
            {
                _eventOutcome.Run();
                _fired = true;
            }
            else if (!playerController.CameraHasTarget()
                     && Input.GetButtonDown("Interact"))
            {
                if (!_eventOutcome.HasFinished) return;
                
                playerCamera.ReturnToLookTarget();
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                State = InteractableState.Post;
            }
        }

        protected override void FirePostEvent()
        {
            if (playerCamera.hasTarget) return;
            
            playerController.State = new Playing(playerController);
            State = InteractableState.Finished;
        }
    }
}