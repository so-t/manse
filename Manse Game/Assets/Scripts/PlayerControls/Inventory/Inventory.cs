using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public UIUtilities uiUtilities;
        public InventoryState.InventoryState state;
        public InventoryDisplay display;
        
        public int displayedItemIndex;
        
        [SerializeField]
        private UnityEngine.Camera displayCamera;
        [SerializeField]
        private List<GameObject> itemList = new List<GameObject>();
        
        private class RotationEvent : UnityEvent<float> {};
        
        private SubtitleDisplay _subtitleDisplay;
        private RotationEvent _rotationEvent;

        public void Add(GameObject obj)
        {
            itemList.Add(obj);
            var rb = obj.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }

        public bool Remove(GameObject obj) { return itemList.Remove(obj); }

        public bool Exit() => state.Exit();

        public bool Contains(string itemName) { return itemList.Any(item => itemName == item.name); }

        public int Count() => itemList.Count;

        public void CreateDisplay()
        {
            if (itemList.Count <= 0) return;
            DestroyDisplay();

            display = new InventoryDisplay(
                itemList,
                parentObject: displayCamera.gameObject
            );
            
            uiUtilities.DimBackground();
            uiUtilities.CreateInventoryControlsDisplay();
            _subtitleDisplay = uiUtilities.CreateSubtitleDisplay();
            
            DisplaySelectedObjectName();
        }

        public void ClearDisplayText() => _subtitleDisplay.ClearText();

        public void DestroyDisplay()
        {
            if (display == null) return;
            
            display.Close();
            display = null;
            displayedItemIndex = 0;
            
            uiUtilities.DestroySubtitleDisplay();
            uiUtilities.DestroyInventoryControlsDisplay();
            uiUtilities.ResetBackgroundBrightness();
        }

        public GameObject GetDisplayedObject() =>  itemList[displayedItemIndex];

        public Vector3 GetDisplayCameraPosition() => displayCamera.transform.position;

        // Could move this to a UI Text field that queries the name from inventory object unless currently rotating.
        // Splits up the functionality too much? Might Keep this space cleaner.
        public void DisplaySelectedObjectName() => _subtitleDisplay.SetText(GetDisplayedObject().name); 

        public void RotateDisplay(float direction)
        {
            state.RotateDisplay(direction);
        }

        public bool InspectObject()
        {
            return state.InspectObject();
        }

        private void Awake()
        {
            state = new InventoryState.InventoryState();
            _rotationEvent = new RotationEvent();
            _rotationEvent.AddListener(RotateDisplay);
        }

        private void Update()
        {
            state.Update();
        }

        private void FixedUpdate()
        {
            state.FixedUpdate();
        }
    }
}