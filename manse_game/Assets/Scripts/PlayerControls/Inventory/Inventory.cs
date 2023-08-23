using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Camera displayCamera;
        
        [SerializeField]
        private List<GameObject> itemList = new List<GameObject>();

        private TMP_Text _textDisplay;
        private InventoryDisplay _display;
        private TeleType _teleType;
        private MeshRenderer _background;
        
        private int _displayedItemIndex;
        
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
        }

        public GameObject GetDisplayedObject() { return itemList[_displayedItemIndex]; }

        private void DisplaySelectedObjectName() { _teleType.SetDisplayText(GetDisplayedObject().name); }
        
        private void ClearDisplayText() { _teleType.SetDisplayText(""); }

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

        private void Awake()
        {
            _textDisplay = displayCamera.GetComponentInChildren<TMP_Text>();
            _teleType = _textDisplay.GetComponent<TeleType>();
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