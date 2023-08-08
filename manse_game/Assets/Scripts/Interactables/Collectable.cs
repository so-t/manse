using Items;
using UnityEngine;

namespace Interactables
{
    public class Collectable : BaseInteractable
    {
        private Item _item;
        private MeshRenderer _objectMesh;

        protected override void Awake()
        {
            base.Awake();
            _objectMesh = gameObject.GetComponent<MeshRenderer>();
            _item = gameObject.GetComponent<Item>();
        }
        
        protected override bool ExitCondition(){ 
            return Fired && !PlayerCamera.hasTarget 
                          && Input.GetButtonDown("Interact")
                          && PlayerController.textField.text == displayText; 
        }

        protected override void Exit()
        {
            _objectMesh.enabled = false;
            PlayerController.AddToInventory(_item);
        }
    }
}