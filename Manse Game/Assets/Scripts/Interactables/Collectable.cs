using UnityEngine;

namespace Interactables
{
    public class Collectable : BaseInteractable
    {
        private MeshRenderer _mesh;
        private Collider _collider;

        protected override void Awake()
        {
            base.Awake();
            _mesh = gameObject.GetComponent<MeshRenderer>();
            _collider = gameObject.GetComponent<Collider>();
        }
        
        protected override bool ExitCondition()
        { 
            return Fired && !PlayerCamera.hasTarget 
                          && Input.GetButtonDown("Interact")
                          && MessageDisplayComplete(); 
        }

        protected override void Exit()
        {
            _mesh.enabled = false;
            _collider.enabled = false;
            PlayerController.AddToInventory(gameObject);
        }
    }
}