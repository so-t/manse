using PlayerControls.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIUtilities : MonoBehaviour
    {
        public GameObject inventoryControlsPrefab;
        public GameObject subtitleDisplayPrefab;
        
        public Camera displayCamera;
        
        [SerializeField]
        private GameObject player;
        
        private GameObject _inventoryControls;
        private GameObject _subtitleDisplay;
        private GameObject _transparentBackground;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = displayCamera.GetComponentInChildren<MeshRenderer>();
        }

        public void DimBackground()
        {
            _meshRenderer.materials[0].color /= 2;
        }

        public void ResetBackgroundBrightness()
        {
            _meshRenderer.materials[0].color *= 2;
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

            var inventory = player.GetComponentInChildren<Inventory>();
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