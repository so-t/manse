using PlayerControls.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIUtilities : MonoBehaviour
    {
        private GameObject _inventoryControls;
        private SubtitleDisplay _subtitleDisplay;
        private MeshRenderer _transparentBackgroundMesh;
        
        [SerializeField]
        private GameObject player;
        [SerializeField]
        private GameObject inventoryControlsPrefab;
        [SerializeField]
        private GameObject subtitleDisplayPrefab;
        
        public Camera displayCamera;
            
        private void Awake()
        {
            _transparentBackgroundMesh = displayCamera.GetComponentInChildren<MeshRenderer>();
            _subtitleDisplay = CreateSubtitleDisplay();
            _inventoryControls = CreateInventoryControlsDisplay();
        }

        public void DimBackground()
        {
            _transparentBackgroundMesh.materials[0].color /= 2;
        }

        public void ResetBackgroundBrightness()
        {
            _transparentBackgroundMesh.materials[0].color *= 2;
        }

        private SubtitleDisplay CreateSubtitleDisplay()
        {
            var subtitleDisplay = Instantiate(subtitleDisplayPrefab, transform, false);
            
            return subtitleDisplay.GetComponent<SubtitleDisplay>();
        }

        public void EnableSubtitleDisplay(bool enableBackground=true)
        {
            _subtitleDisplay.gameObject.SetActive(true);
            if (enableBackground) _subtitleDisplay.EnableBackground();
        }

        public void DisableSubtitleDisplay()
        {
            _subtitleDisplay.gameObject.SetActive(false);
            _subtitleDisplay.DisableBackground();
        }

        public bool SubtitleTextMatches(string str)
        {
            return _subtitleDisplay.DisplayMessageMatches(str);
        }

        public void SetSubtitleText(string str)
        {
            if (!_subtitleDisplay.gameObject.activeSelf)
                EnableSubtitleDisplay();
            
            _subtitleDisplay.SetText(str);
        }

        public void ClearSubtitleText()
        {
            if (!_subtitleDisplay.gameObject.activeSelf)
                EnableSubtitleDisplay();
            
            _subtitleDisplay.ClearText();
        }

        public void TeleTypeMessage(string str)
        {
            if (!_subtitleDisplay.gameObject.activeSelf)
                EnableSubtitleDisplay();

            StartCoroutine(_subtitleDisplay.TeleTypeMessage(str));
        }

        private GameObject CreateInventoryControlsDisplay()
        {
            var inventoryControls = Instantiate(inventoryControlsPrefab, transform, false);

            var inventory = player.GetComponentInChildren<Inventory>();
            foreach (var button in inventoryControls.GetComponentsInChildren<Button>())
            {
                if (button.name.Contains("Right"))
                {
                    button.onClick.AddListener(() =>
                        inventory.RotateDisplay(1));
                    button.gameObject.SetActive(false);
                }
                else if (button.name.Contains("Left"))
                {
                    button.onClick.AddListener(() =>
                        inventory.RotateDisplay(-1));
                    button.gameObject.SetActive(false);
                }
            }

            return inventoryControls;
        }

        public void EnableInventoryControlsDisplay()
        {
            _inventoryControls.SetActive(true);
        }

        public void DisableInventoryControlsDisplay()
        {
            _inventoryControls.SetActive(false);
        }
    }
}