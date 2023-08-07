using Items;
using PlayerControls.PlayerState;
using UI;
using UnityEngine;

namespace Interactables
{
    public class Collectable : Interactable
    {
        public string str = "";

        private bool _fired;

        private TeleType _t;

        private Item _item;
        
        private void Awake()
        {
            _item = gameObject.GetComponent<Item>();
        }

        protected override void CheckTrigger()
        {
            if (PlayerInRange(cutoff) 
                && Input.GetButtonDown("Interact") 
                && playerController.Interact(transform))
                State = InteractableState.Triggered;
        }

        protected override void FireEvent()
        {
            switch (playerCamera.hasTarget)
            {
                case false when !_fired:
                    _t = playerController.CreateTeleType(str);
                    _fired = true;
                    break;
                case false when Input.GetButtonDown("Interact"):
                {
                    if (_t == null || !_t.HasFinished()) return;
            
                    _t.Clear();
                    playerCamera.ReturnToLookTarget();
                    _fired = false;
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    playerController.AddToInventory(_item);
                    State = InteractableState.Post;
                    break;
                }
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