using System.Collections.Generic;
using Azylon.Data;

namespace Azylon.Purchasing
{
    public interface IShopService
    {
        IReadOnlyList<ItemDataSO> GetAvailableItemsList();
        PurchaseResult TryPurchaseItem(ItemDataSO itemId);
    }
}