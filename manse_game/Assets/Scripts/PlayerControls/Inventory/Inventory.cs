using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        
        public Button leftButton;
        public Button rightButton;
        public SubtitleDisplay subtitleDisplay;
        
        [SerializeField]
        private UnityEngine.Camera displayCamera;
        
        [SerializeField]
        private List<GameObject> itemList = new List<GameObject>();
        private InventoryDisplay _display;
        private MeshRenderer _background;
        
        private int _displayedItemIndex;

        private class RotationEvent : UnityEvent<float> {};
        private RotationEvent _rotationEvent;

        public void Add(GameObject obj) { itemList.Add(obj); }

        public bool Remove(GameObject obj) { return itemList.Remove(obj); }

        public bool Contains(string itemName) { return itemList.Any(item => itemName == item.name); }

        public void CreateDisplay()
        {
            if (itemList.Count <= 0) return;
            DestroyDisplay();

            _display = new InventoryDisplay(
                itemList,
                parentObject: displayCamera.gameObject
            );
            
            leftButton.gameObject.SetActive(true);
            rightButton.gameObject.SetActive(true);

            _background.enabled = true;
            DisplaySelectedObjectName();
        }

        public void DestroyDisplay()
        {
            if (_display == null) return;
            
            _display.Close();
            _display = null;
            _displayedItemIndex = 0;
            ClearDisplayText();
            _background.enabled = false;
            
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
        }

        public GameObject GetDisplayedObject() { return itemList[_displayedItemIndex]; }

        private void DisplaySelectedObjectName() { subtitleDisplay.SetText(GetDisplayedObject().name); }
        
        private void ClearDisplayText() { subtitleDisplay.SetText(""); }

        public void SetRotationDir(float direction)
        {
            if (_display == null || _display.IsRotating() || direction == 0.0f) return;
            
            _display.SetRotationDirection(direction);
            _displayedItemIndex = (direction < 0.0f) switch
            {
                true => _displayedItemIndex + 1 >= itemList.Count ? 0 : _displayedItemIndex + 1,
                false => _displayedItemIndex - 1 < 0 ? itemList.Count - 1 : _displayedItemIndex - 1
            };
        }

        private void Awake()
        {
            _rotationEvent = new RotationEvent();
            _rotationEvent.AddListener(SetRotationDir);
            
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
            
            foreach (var meshRenderer in displayCamera.GetComponentsInChildren<MeshRenderer>())
            {
                if (meshRenderer.gameObject.name == "Transparent Background") _background = meshRenderer;
            }
        }

        private void FixedUpdate()
        {
            if (_display == null) return;

            if (_display.IsRotating())
            {
                if (_display.HasFinishedRotating())
                {
                    _display.StopRotating();
                    DisplaySelectedObjectName();
                }
                else
                {
                    _display.Rotate();
                    ClearDisplayText();
                }
            }
            else
            {
                InventoryDisplay.RotateDisplayObject(itemList[_displayedItemIndex]);
            }
        }
    }
}