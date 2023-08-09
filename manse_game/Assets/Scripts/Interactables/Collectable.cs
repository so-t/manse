using Items;
using UnityEngine;

namespace Interactables
{
    public class Collectable : BaseInteractable
    {
        private Item _item;
        private MeshRenderer _mesh;
        private Collider _collider;

        protected override void Awake()
        {
            base.Awake();
            _mesh = gameObject.GetComponent<MeshRenderer>();
            _collider = gameObject.GetComponent<Collider>();
            _item = gameObject.GetComponent<Item>();
        }
        
        protected override bool ExitCondition(){ 
            return Fired && !PlayerCamera.hasTarget 
                          && Input.GetButtonDown("Interact")
                          && PlayerController.textField.text == displayText; 
        }

        protected override void Exit()
        {
            _mesh.enabled = false;
            _collider.enabled = false;
            PlayerController.AddToInventory(_item);
        }
    }
}