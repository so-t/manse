using UnityEngine;

namespace Interactables
{
    internal class InspectionPoint : BaseInteractable
    {
        protected override bool ExitCondition(){ 
            return fired && !playerCamera.hasTarget 
                         && Input.GetButtonDown("Interact")
                         && MessageDisplayComplete();
        }
    }
}