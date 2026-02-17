using System.Collections.Generic;
using Azylon.Currency;
using Azylon.Data;
using Azylon.Inventory;
using Azylon.ItemRepository;

namespace Azylon.Purchasing
{
    public class ShopService : IShopService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICurrencyService _currencyService;
        private readonly IInventoryService _inventoryService;

        public ShopService(IItemRepository itemRepository, ICurrencyService currencyService,
            IInventoryService inventoryService)
        {
            _itemRepository = itemRepository;
            _currencyService = currencyService;
            _inventoryService = inventoryService;
        }

        public IReadOnlyList<ItemDataSO> GetAvailableItemsList() =>
            _itemRepository.GetAllItemsList();

        public PurchaseResult TryPurchaseItem(ItemDataSO itemId)
        {
            var item = _itemRepository.GetItemById(itemId.GetId());
            if (item == null)
                return PurchaseResult.ItemAlreadyOwned;
            if (!_currencyService.TrySpend(item.Price))
                return PurchaseResult.InsufficientFunds;

            _inventoryService.AddItem(item.GetId());
            return PurchaseResult.Success;
        }
    }
}