using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private List<Item> inventory = new List<Item>();

        public void Add(Item item) { inventory.Add(item); }

        public bool Remove(Item item) { return inventory.Remove(item); }

        public bool Contains(string itemName) { return inventory.Any(item => itemName == item.itemName); }

    }
}