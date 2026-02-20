using System.Collections.Generic;

namespace Azulon.ItemRepository
{
    public interface IItemRepository
    {
        IReadOnlyList<ItemDataSO> GetAllItemsList();
        ItemDataSO GetItemById(string id);
    }
}