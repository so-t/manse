using Items;
using PlayerControls.PlayerState;
using UnityEngine;

namespace Interactables
{
    public class Collectable : Interactable
    {
        public string str = "";
        private Item _item;

        protected override void Awake()
        {
            base.Awake();
            cameraTarget = transform;
            _item = gameObject.GetComponent<Item>();
        }
        
        protected override void Action()
        {
            TeleType = playerController.CreateTeleType(str);
        }
        
        protected override bool ExitCondition(){ 
            return Fired && !playerCamera.hasTarget 
                          && Input.GetButtonDown("Interact")
                          && TeleType.HasFinished(); 
        }

        protected override void ActionExit()
        {
            TeleType.Clear();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            playerController.AddToInventory(_item);
        }

        protected override void FirePostAction()
        {
            playerController.State = new Playing(playerController);
        }
    }
}