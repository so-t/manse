using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Camera displayCamera;
        
        [SerializeField]
        private List<GameObject> itemList = new List<GameObject>();
        
        private InventoryDisplay _display;
        
        private int _displayedItemIndex;
        
        public void Add(GameObject obj) { itemList.Add(obj); }

        public bool Remove(GameObject obj) { return itemList.Remove(obj); }

        public bool Contains(string itemName) { return itemList.Any(item => itemName == item.name); }

        public void CreateDisplay()
        {
            if (_display != null) DestroyDisplay();
            
            _display = new InventoryDisplay(
                itemList,
                parentObject: displayCamera.gameObject
                );
        }

        public void DestroyDisplay()
        {
            _display.Close();
            _display = null;
            _displayedItemIndex = 0;
        }

        public GameObject GetDisplayedObject() { return itemList[_displayedItemIndex]; }

        public void RotateDisplay(float direction)
        {
            if (_display == null || _display.IsRotating() || direction == 0.0f) return;
            
            _display.SetRotationDirection(direction);
            _displayedItemIndex = (direction < 0.0f) switch
            {
                true => _displayedItemIndex + 1 >= itemList.Count ? 0 : _displayedItemIndex + 1,
                false => _displayedItemIndex - 1 < 0 ? itemList.Count - 1 : _displayedItemIndex - 1
            };
        }

        private void FixedUpdate()
        {
            if (_display == null) return;

            if (_display.IsRotating()){
                if (_display.HasFinishedRotating())
                    _display.StopRotating();
                else
                    _display.Rotate();
            }
            else
            {
                InventoryDisplay.RotateDisplayObject(itemList[_displayedItemIndex]);
            }
        }
    }
}