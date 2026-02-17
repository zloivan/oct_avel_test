using System.Collections.Generic;

namespace Azylon.ItemRepository
{
    public class ScriptableObjectItemRepository : IItemRepository
    {
        private readonly Dictionary<string, ItemDataSO> _itemsById;
        private readonly IReadOnlyList<ItemDataSO> _allItemsList;

        public ScriptableObjectItemRepository(ItemDataSO[] items)
        {
            _itemsById = new Dictionary<string, ItemDataSO>(items.Length);
            foreach (var item in items)
            {
                _itemsById.Add(item.GetId(), item);
            }

            _allItemsList = new List<ItemDataSO>(_itemsById.Values).AsReadOnly();
        }

        public IReadOnlyList<ItemDataSO> GetAllItemsList() =>
            _allItemsList;


        public ItemDataSO GetItemById(string id) =>
            _itemsById[id];
    }
}