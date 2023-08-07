using Items;
using PlayerControls.Controller;
using PlayerControls.Camera;
using PlayerControls.PlayerState;
using PlayerControls.Inventory;
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
                player.GetComponentInChildren<Inventory>().Add(_item);
                player.GetComponentInChildren<CameraRotation>().ReturnToLookTarget();
                _fired = false;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                State = InteractableState.Post;
            }
        }

        protected override void FirePostEvent()
        {
            if (!player.GetComponentInChildren<CameraRotation>().hasTarget)
            {
                player.GetComponent<PlayerController>().State = new Playing(player);
                State = InteractableState.Finished;
            }
        }
    }
}