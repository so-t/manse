using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SubtitleDisplay : MonoBehaviour
    {
        private const float Delay = 0.05f;
        
        private TMP_Text _tmp;
        private GameObject _backgroundObject;

        private void Awake()
        {
            _tmp = gameObject.GetComponentsInChildren<TMP_Text>()[0];
            _backgroundObject = gameObject.GetComponentsInChildren<Image>()[0].gameObject;
            DisableBackground();
        }
        
        public void SetText(string str) { _tmp.text = str; }

        public void ClearText() { _tmp.text = ""; }

        public bool DisplayMessageMatches(string str)
        {
            return _tmp.text == str;
        }

        public IEnumerator TeleTypeMessage(string str)
        {
            var display = "";
            if (!_backgroundObject.gameObject.activeInHierarchy) EnableBackground();

            foreach (var c in str)
            {
                display += c;
                SetText(display);

                yield return new WaitForSeconds(Delay);
            }
        }
        
        public void EnableBackground() { _backgroundObject.gameObject.SetActive(true); }

        public void DisableBackground() { _backgroundObject.gameObject.SetActive(false); }
    }
}