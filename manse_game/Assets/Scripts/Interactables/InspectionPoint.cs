using PlayerControls.PlayerState;
using UnityEngine;

namespace Interactables
{
    internal class InspectionPoint : Interactable
    {
        public string str = "";

        protected override void Awake()
        {
            base.Awake();
            cameraTarget = transform;
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
        }

        protected override void FirePostAction()
        {
            playerController.State = new Playing(playerController);
        }
    }
}