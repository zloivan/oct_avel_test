using System.Collections.Generic;

namespace Azylon.ItemRepository
{
    public interface IItemRepository
    {
        IReadOnlyList<ItemDataSO> GetAllItemsList();
        ItemDataSO GetItemById(string id);
    }
}