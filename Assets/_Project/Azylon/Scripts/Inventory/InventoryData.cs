using System;
using System.Collections.Generic;

namespace Azylon.Inventory
{
    [Serializable]
    public class InventoryData
    {
        public List<string> OwnedItemsIdList = new();
    }
}