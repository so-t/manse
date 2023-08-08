using PlayerControls.PlayerState;
using UnityEngine;

namespace Interactables
{
    internal class InspectionPoint : BaseInteractable
    {
        public string str = "";

        protected override void Awake()
        {
            base.Awake();
            lookTarget = transform;
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
        }

        protected override void FirePostAction()
        {
            PlayerController.State = new Playing(PlayerController);
        }
    }
}