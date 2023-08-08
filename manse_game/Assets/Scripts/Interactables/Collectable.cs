using Items;
using PlayerControls.PlayerState;
using UnityEngine;

namespace Interactables
{
    public class Collectable : BaseInteractable
    {
        public string str = "";
        private Item _item;

        protected override void Awake()
        {
            base.Awake();
            lookTarget = transform;
            _item = gameObject.GetComponent<Item>();
        }
        
        protected override void Action()
        {
            TeleType = PlayerController.CreateTeleType(str);
        }
        
        protected override bool ExitCondition(){ 
            return Fired && !PlayerCamera.hasTarget 
                          && Input.GetButtonDown("Interact")
                          && TeleType.HasFinished(); 
        }

        protected override void Exit()
        {
            TeleType.Clear();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            PlayerController.AddToInventory(_item);
        }

        protected override void FirePostAction()
        {
            PlayerController.State = new Playing(PlayerController);
        }
    }
}