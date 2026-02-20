using System;
using System.Collections.Generic;
using Azulon.SaveData;

namespace Azulon.Inventory
{
    public class InventoryService : IInventoryService
    {
        public event Action OnInventoryChanged;

        private readonly InventoryOrganizer _organizer;

        public InventoryService(InventoryOrganizer organizer) =>
            _organizer = organizer;

        public IReadOnlyList<string> OwnedItemsIdList() =>
            _organizer.GetOwnedItems();

        public bool HasItem(string itemId) =>
            _organizer.HasItem(itemId);

        public void AddItem(string itemId)
        {
            if (_organizer.HasItem(itemId)) return;
            _organizer.PlaceItem(itemId);
            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(string itemId)
        {
            if (!_organizer.HasItem(itemId)) return;
            _organizer.RemoveItem(itemId);
            OnInventoryChanged?.Invoke();
        }
    }
}