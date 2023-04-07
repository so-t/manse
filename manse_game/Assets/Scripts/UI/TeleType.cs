using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleType : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public string str;

    public TeleType(TMP_Text textMeshPro, string str)
    {
        this.textMeshPro = textMeshPro;
        this.str = str;
    }

    public void Clear()
    {
        textMeshPro.text = "";
    }

    public bool HasFinished()
    {
        return textMeshPro.text.Length == str.Length;
    }

    IEnumerator Start()
    {
        string display = "";

        foreach (char c in str){
            display = display + c;
            textMeshPro.text = display;

            yield return new WaitForSeconds(0.05f);
        }
    }
}