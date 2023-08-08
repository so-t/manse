using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TeleType : MonoBehaviour
    {
        private const float Delay = 0.05f;
        private TMP_Text _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        public void Clear()
        {
            _textMeshPro.text = "";
        }

        public IEnumerator DisplayMessage(string str)
        {
            var display = "";

            foreach (var c in str)
            {
                display += c;
                _textMeshPro.text = display;

                yield return new WaitForSeconds(Delay);
            }
        }
    }
}