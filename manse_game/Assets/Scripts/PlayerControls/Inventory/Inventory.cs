using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Camera displayCamera;
        [SerializeField]
        private List<GameObject> inventory = new List<GameObject>();
        private int _displayIndex = 1;

        private InventoryDisplay _display;
        
        private int _itemCount = 2; // TODO: Remove 
        
        public void Add(GameObject obj) { inventory.Add(obj); }

        public bool Remove(GameObject obj) { return inventory.Remove(obj); }

        public bool Contains(string itemName) { return inventory.Any(item => itemName == item.name); }

        public GameObject GetDisplayedObject()
        {
            return inventory[_displayIndex];
        }

        public void CreateDisplay()
        {
            _display = new InventoryDisplay(
                itemCount: _itemCount, // TODO: Change this to `inventory.Count`
                parentObject: displayCamera.gameObject
                );
            _itemCount++; // TODO: Remove 
        }

        public void DestroyDisplay()
        {
            _display.Close();
            _display = null;
        }

        public void Rotate(float direction)
        {
            if (_display == null || _display.IsRotating() || direction == 0.0f) return;
            
            _display.SetRotationDirection(direction);
            _displayIndex = (direction > 0.0f) switch
            {
                true => _displayIndex + 1 >= inventory.Count - 1 ? 1 : _displayIndex + 1,
                false => _displayIndex - 1 < 1 ? inventory.Count - 1 : _displayIndex - 1
            };
        }

        private void FixedUpdate()
        {
            if (_display == null || !_display.IsRotating()) return;

            if (_display.HasFinishedRotating())
                _display.StopRotating();
            else
                _display.Rotate();
        }
    }
}