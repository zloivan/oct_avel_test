using System;
using System.Collections.Generic;

namespace Azulon.Inventory
{
    [Serializable]
    public class InventoryData
    {
        public List<string> OwnedItemsIdList = new();
    }
}