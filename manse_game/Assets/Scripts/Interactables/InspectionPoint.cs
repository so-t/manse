using PlayerControls.Camera;
using PlayerControls.Controller;
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
                && player.GetComponent<PlayerController>().Interact(transform)) 
                State = InteractableState.Triggered;
        }

        protected override void FireEvent()
        {   
            if (!player.GetComponentInChildren<CameraRotation>().hasTarget && !_fired)
            {
                _t = player.GetComponent<PlayerController>().textField.gameObject.AddComponent<TeleType>();
                _t.enabled = false;
                _t.str = this.str;
                _t.textMeshPro = player.GetComponent<PlayerController>().textField;
                _t.enabled = true;
                _fired = true;
            }
            else if (!player.GetComponentInChildren<CameraRotation>().hasTarget && Input.GetButtonDown("Interact"))
            {
                if (_t == null || !_t.HasFinished()) return;
                _t.Clear();
                player.GetComponentInChildren<CameraRotation>().ReturnToLookTarget();
                _fired = false;
                State = InteractableState.Post;
            }
        }

        protected override void FirePostEvent()
        {
            if (player.GetComponentInChildren<CameraRotation>().hasTarget) return;
            player.GetComponent<PlayerController>().State = new Playing(player);
            State = InteractableState.Finished;
        }
    }
}