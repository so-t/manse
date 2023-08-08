using PlayerControls.PlayerState;
using UnityEngine;

namespace Interactables
{
    internal class InspectionPoint : BaseInteractable
    {
        protected override bool ExitCondition(){ 
            return Fired && !PlayerCamera.hasTarget 
                         && Input.GetButtonDown("Interact")
                         && TeleType.HasFinished(); 
        }

        protected override void FirePostAction()
        {
            PlayerController.State = new Playing(PlayerController);
        }
    }
}