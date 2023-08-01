using System.Collections.Generic;
using Interactables;
using Items;
using UnityEngine;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private List<Item> _inventory = new List<Item>{};

        public void AddToInventory(Interactable target)
        {
            Debug.Log(target.gameObject.name);
            Debug.Log(_inventory[0]);
        }
    }
}