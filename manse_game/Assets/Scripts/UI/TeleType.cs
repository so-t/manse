using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TeleType : MonoBehaviour
    {
        public TMP_Text textMeshPro;
        public float delay;
        public string str;

        public TeleType(TMP_Text textMeshPro, string str, float delay=0.05f)
        {
            this.textMeshPro = textMeshPro;
            this.str = str;
            this.delay = delay;
        }

        public void Clear()
        {
            textMeshPro.text = "";
        }

        public bool HasFinished()
        {
            return textMeshPro.text.Length == str.Length;
        }

        private IEnumerator Start()
        {
            string display = "";

            foreach (char c in str)
            {
                display = display + c;
                textMeshPro.text = display;

                yield return new WaitForSeconds(delay);
            }
        }
    }
}