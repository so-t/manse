using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private List<string> inventory = new List<string>(){"test"};

    public void AddToInventory(Interactable target)
    {
        Debug.Log(target.gameObject.name);
        Debug.Log(inventory[0]);
    }
}