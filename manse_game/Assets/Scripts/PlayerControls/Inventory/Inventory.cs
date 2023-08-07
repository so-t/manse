using System.Collections.Generic;
using Interactables;
using Items;
using UnityEngine;

namespace PlayerControls.Inventory
{
    public class Inventory
    {
        private List<Item> _inventory = new List<Item>();

        public void Add(Item item) { _inventory.Add(item); }
    }
}