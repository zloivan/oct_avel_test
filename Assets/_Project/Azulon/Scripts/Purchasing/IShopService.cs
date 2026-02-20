using System.Collections.Generic;
using Azulon.ItemRepository;

namespace Azulon.Purchasing
{
    public interface IShopService
    {
        IReadOnlyList<ItemDataSO> GetAvailableItemsList();
        PurchaseResult TryPurchaseItem(ItemDataSO itemId);
    }
}