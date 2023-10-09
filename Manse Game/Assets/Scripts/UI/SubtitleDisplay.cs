using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class SubtitleDisplay : MonoBehaviour
    {
        private const float Delay = 0.05f;
        
        public TMP_Text tmp;
        public GameObject background;

        private void Awake()
        {
            background.SetActive(false);
        }
        
        public void SetText(string str) { tmp.text = str; }

        public void ClearText() { tmp.text = ""; }

        public IEnumerator TeleTypeMessage(string str)
        {
            var display = "";
            if (!background.activeInHierarchy) EnableBackground();

            foreach (var c in str)
            {
                display += c;
                SetText(display);

                yield return new WaitForSeconds(Delay);
            }
        }

        public void Disable()
        {
            tmp.text = "";
            DisableBackground();
        }
        
        private void EnableBackground() { background.SetActive(true); }

        private void DisableBackground() { background.SetActive(false); }
    }
}