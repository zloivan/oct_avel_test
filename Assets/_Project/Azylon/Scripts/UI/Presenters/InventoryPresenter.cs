using System.Collections.Generic;
using Azylon.Inventory;
using Azylon.ItemRepository;
using Azylon.UI.UIStates;
using Azylon.UI.Views;

namespace Azylon.UI.Presenters
{
    public class InventoryPresenter
    {
        private readonly InventoryScreenView _view;
        private readonly IInventoryService _inventoryService;
        private readonly IItemRepository _itemRepository;
        private readonly UIStateMachine _stateMachine;

        public InventoryPresenter(InventoryScreenView view, IInventoryService inventoryService,
            IItemRepository itemRepository, UIStateMachine stateMachine)
        {
            _view = view;
            _inventoryService = inventoryService;
            _itemRepository = itemRepository;
            _stateMachine = stateMachine;
        }

        public void Enable()
        {
            RefreshView();
            _inventoryService.OnInventoryChanged += RefreshView;
            _view.OnBackRequested += HandleBack;
            _view.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _inventoryService.OnInventoryChanged -= RefreshView;
            _view.OnBackRequested -= HandleBack;
            _view.gameObject.SetActive(false);
        }

        private void RefreshView()
        {
            var items = _inventoryService.OwnedItemsIdList();
            var ownedItems = new List<ItemDataSO>();
            foreach (var itemId in items)
            {
                var item = _itemRepository.GetItemById(itemId);
                if (item != null)
                    ownedItems.Add(item);
            }

            _view.SetItems(ownedItems);
        }

        private void HandleBack()
        {
            //TODO: Implement stack in state machine
            _stateMachine.SwitchTo<ShopState>();
        }
    }
}