using Items;
using PlayerControls.PlayerState;
using UnityEngine;

namespace Interactables
{
    public class Collectable : BaseInteractable
    {
        private Item _item;

        protected override void Awake()
        {
            base.Awake();
            _item = gameObject.GetComponent<Item>();
        }
        
        protected override bool ExitCondition(){ 
            return Fired && !PlayerCamera.hasTarget 
                          && Input.GetButtonDown("Interact")
                          && TeleType.HasFinished(); 
        }

        protected override void Exit()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            PlayerController.AddToInventory(_item);
        }

        protected override void FirePostAction()
        {
            PlayerController.State = new Playing(PlayerController);
        }
    }
}