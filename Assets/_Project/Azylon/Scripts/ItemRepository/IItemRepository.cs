using System.Collections.Generic;
using Azylon.Data;

namespace Azylon.ItemRepository
{
    public interface IItemRepository
    {
        IReadOnlyList<ItemDataSO> GetAllItemsList();
        ItemDataSO GetItemById(string id);
    }
}