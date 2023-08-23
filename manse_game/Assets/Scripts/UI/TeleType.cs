using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TeleType : MonoBehaviour
    {
        private const float Delay = 0.05f;
        private TMP_Text _textMeshPro;
        private MeshRenderer _bgMesh;

        private void Awake()
        {
            _textMeshPro = GetComponent<TMP_Text>();
            _bgMesh = gameObject.GetComponentInChildren<MeshRenderer>();
        }

        public void Clear()
        {
            _textMeshPro.text = "";
            DisableBackgroundRender();
        }

        public void EnableBackgroundRender() { _bgMesh.enabled = true; }

        public void DisableBackgroundRender() { _bgMesh.enabled = false; }

        public void SetDisplayText(string str) { _textMeshPro.text = str; }

        public IEnumerator DisplayMessage(string str)
        {
            var display = "";
            if (!_bgMesh.enabled) EnableBackgroundRender();

            foreach (var c in str)
            {
                display += c;
                SetDisplayText(display);

                yield return new WaitForSeconds(Delay);
            }
        }
    }
}