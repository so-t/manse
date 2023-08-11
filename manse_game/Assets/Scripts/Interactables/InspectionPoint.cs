using UnityEngine;

namespace Interactables
{
    internal class InspectionPoint : BaseInteractable
    {
        protected override bool ExitCondition(){ 
            return Fired && !PlayerCamera.hasTarget 
                         && Input.GetButtonDown("Interact")
                         && PlayerController.textField.text == displayText;
        }
    }
}