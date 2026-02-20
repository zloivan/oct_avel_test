using System;
using System.Collections.Generic;
using Azulon.SaveData;

namespace Azulon.Inventory
{
    public class InventoryService : IInventoryService
    {
        public event Action OnInventoryChanged;

        private const string SAVE_KEY = "InventoryItems";

        private readonly List<string> _ownedItemsIdList;
        private readonly ISaveSystem _saveSystem;

        public InventoryService(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _ownedItemsIdList = _saveSystem.SaveExists(SAVE_KEY)
                ? _saveSystem.Load<InventoryData>(SAVE_KEY).OwnedItemsIdList
                : new List<string>();
        }

        public IReadOnlyList<string> OwnedItemsIdList() =>
            _ownedItemsIdList;

        public bool HasItem(string itemId) =>
            _ownedItemsIdList.Contains(itemId);

        public void AddItem(string itemId)
        {
            if (_ownedItemsIdList.Contains(itemId))
                return;

            _ownedItemsIdList.Add(itemId);
            _saveSystem.Save(SAVE_KEY, new InventoryData { OwnedItemsIdList = _ownedItemsIdList });
            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(string itemId)
        {
            if (!_ownedItemsIdList.Contains(itemId))
                return;

            _ownedItemsIdList.Remove(itemId);
            _saveSystem.Save(SAVE_KEY, new InventoryData { OwnedItemsIdList = _ownedItemsIdList });
            OnInventoryChanged?.Invoke();
        }
    }
}