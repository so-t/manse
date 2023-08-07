using System.Collections.Generic;
using Items;

namespace PlayerControls.Inventory
{
    public class Inventory
    {
        private List<Item> _inventory = new List<Item>();

        public void Add(Item item) { _inventory.Add(item); }

        public bool Remove(Item item) { return _inventory.Remove(item); }
        
    }
}