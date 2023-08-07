using PlayerControls.PlayerState;
using UI;
using UnityEngine;

namespace Interactables
{
    internal class InspectionPoint : Interactable
    {
        public string str = "";
    
        private bool _fired;

        private TeleType _t;

        protected override void CheckTrigger()
        {
            if (PlayerInRange(cutoff) 
                && Input.GetButtonDown("Interact") 
                && playerController.Interact(transform)) 
                State = InteractableState.Triggered;
        }

        protected override void FireEvent()
        {   
            if (!playerCamera.hasTarget && !_fired)
            {
                _t = playerController.CreateTeleType(str);
                _fired = true;
            }
            else if (!playerCamera.hasTarget 
                     && Input.GetButtonDown("Interact"))
            {
                if (_t == null || !_t.HasFinished()) return;
                _t.Clear();
                playerCamera.ReturnToLookTarget();
                _fired = false;
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