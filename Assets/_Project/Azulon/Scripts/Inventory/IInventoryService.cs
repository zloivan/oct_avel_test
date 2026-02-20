using System;
using System.Collections.Generic;

namespace Azulon.Inventory
{
    public interface IInventoryService
    {
        event Action OnInventoryChanged;

        IReadOnlyList<string> OwnedItemsIdList();
        bool HasItem(string itemId);
        void AddItem(string itemId);
        void RemoveItem(string itemId);
    }
}