using System.Collections.Generic;
using Interactables;
using UnityEngine;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private List<string> _inventory = new List<string>{"test"};

        public void AddToInventory(Interactable target)
        {
            Debug.Log(target.gameObject.name);
            Debug.Log(_inventory[0]);
        }
    }
}