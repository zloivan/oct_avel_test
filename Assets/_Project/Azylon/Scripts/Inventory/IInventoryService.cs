using System;
using System.Collections.Generic;

namespace Azylon.Inventory
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