using System.Collections.Generic;
using Azylon.ItemRepository;

namespace Azylon.Purchasing
{
    public interface IShopService
    {
        IReadOnlyList<ItemDataSO> GetAvailableItemsList();
        PurchaseResult TryPurchaseItem(ItemDataSO itemId);
    }
}