using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Camera displayCamera;
        [SerializeField]
        private List<Item> inventory = new List<Item>();
        private int _displayIndex = 1;

        private InventoryDisplay _display;
        
        public void Add(Item item) { inventory.Add(item); }

        public bool Remove(Item item) { return inventory.Remove(item); }

        public bool Contains(string itemName) { return inventory.Any(item => itemName == item.itemName); }

        public void CreateDisplay()
        {
            _display = new InventoryDisplay(inventory.Count, displayCamera);
        }

        public void DestroyDisplay()
        {
            _display.Close();
            _display = null;
        }

        private void Update()
        {
            if (_display == null || Input.GetAxisRaw("Horizontal") == 0.0f || _display.IsRotating()) return;
            
            var direction = Input.GetAxisRaw("Horizontal") < 0.0f ? 
                InventoryDisplay.RotatingLeft 
                : InventoryDisplay.RotatingRight;
            _display.SetRotationDirection(direction);
                
            _displayIndex = direction switch
            {
                -1 => _displayIndex - 1 < 1 ? inventory.Count - 1 : _displayIndex - 1,
                1 => _displayIndex + 1 >= inventory.Count - 1 ? 1 : _displayIndex + 1,
                _ => throw new ArgumentOutOfRangeException()
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