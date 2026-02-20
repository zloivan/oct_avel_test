using System;
using System.Collections.Generic;
using System.Linq;
using Azulon.SaveData;

namespace Azulon.Inventory
{
    public class InventoryOrganizer
    {
        public const int SLOT_COUNT = 16;
        private const string SAVE_KEY = "inventory_slots";

        private readonly string[] _slotsArray;
        private readonly ISaveSystem _saveSystem;

        public InventoryOrganizer(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _slotsArray = new string[SLOT_COUNT];
            Load();
        }

        public string GetSlotItem(int index) =>
            _slotsArray[index];

        public void PlaceItem(string itemId)
        {
            var idx = Array.IndexOf(_slotsArray, null);
            if (idx < 0)
                return;
            _slotsArray[idx] = itemId;
            Save();
        }

        public void RemoveItem(string itemId)
        {
            var idx = Array.IndexOf(_slotsArray, itemId);
            if (idx < 0)
                return;
            _slotsArray[idx] = null;
            Save();
        }

        public void SwapSlots(int from, int to)
        {
            if (from == to)
                return;

            (_slotsArray[from], _slotsArray[to]) = (_slotsArray[to], _slotsArray[from]);
            Save();
        }

        public bool HasItem(string itemId) =>
            Array.IndexOf(_slotsArray, itemId) >= 0;

        public IReadOnlyList<string> GetOwnedItems() =>
            _slotsArray.Where(slot => slot != null).ToList();

        private void Save() =>
            _saveSystem.Save(SAVE_KEY, new InventoryOrganizerData { SlotIdsArray = _slotsArray });

        private void Load()
        {
            var data = _saveSystem.Load<InventoryOrganizerData>(SAVE_KEY);
            if (data?.SlotIdsArray == null)
                return;

            var slotsLength = Math.Min(data.SlotIdsArray.Length, SLOT_COUNT);
            Array.Copy(data.SlotIdsArray, _slotsArray, slotsLength);
        }
    }
}