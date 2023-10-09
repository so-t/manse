using PlayerControls.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIUtilities : MonoBehaviour
    {
        public GameObject transparentBackgroundPrefab;
        private GameObject _transparentBackground;
        
        public GameObject subtitleDisplayPrefab;
        private GameObject _subtitleDisplay;

        public GameObject inventoryControlsPrefab;
        private GameObject _inventoryControls;

        [SerializeField]
        private GameObject _player;

        public void CreateTransparentBackground()
        {
            _transparentBackground = Instantiate(transparentBackgroundPrefab, transform, false);
        }

        public void DestroyTransparentBackground()
        {
            Destroy(_transparentBackground);   
        }

        public SubtitleDisplay CreateSubtitleDisplay(bool enableBackground=true)
        {
            _subtitleDisplay = Instantiate(subtitleDisplayPrefab, transform, false);
            var subtitleDisplay = _subtitleDisplay.GetComponent<SubtitleDisplay>();
            subtitleDisplay.background.SetActive(enableBackground);
            
            return subtitleDisplay;
        }

        public void DestroySubtitleDisplay()
        {
            Destroy(_subtitleDisplay);
        }

        public void CreateInventoryControlsDisplay()
        {
            _inventoryControls = Instantiate(inventoryControlsPrefab, transform, false);

            var inventory = _player.GetComponentInChildren<Inventory>();
            foreach (var button in _inventoryControls.GetComponentsInChildren<Button>())
            {
                if (button.name.Contains("Right")) 
                    button.onClick.AddListener(() => 
                        inventory.SetRotationDir(1));
                else if (button.name.Contains("Left")) 
                    button.onClick.AddListener(() => 
                        inventory.SetRotationDir(-1));
            }
        }

        public void DestroyInventoryControlsDisplay()
        {
            Destroy(_inventoryControls);
        }
    }
}