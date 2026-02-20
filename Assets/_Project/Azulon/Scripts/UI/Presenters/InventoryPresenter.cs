using Azulon.Inventory;
using Azulon.ItemRepository;
using Azulon.UI.UIStates;
using Azulon.UI.Views;

namespace Azulon.UI.Presenters
{
    public class InventoryPresenter
    {
        private readonly InventoryScreenView _view;
        private readonly IInventoryService _inventoryService;
        private readonly IItemRepository _itemRepository;
        private readonly InventoryOrganizer _organizer;

        private UIStateMachine _stateMachine;

        public InventoryPresenter(InventoryScreenView view, IInventoryService inventoryService,
            IItemRepository itemRepository, InventoryOrganizer organizer)
        {
            _view = view;
            _inventoryService = inventoryService;
            _itemRepository = itemRepository;
            _organizer = organizer;
        }

        public void Inject(UIStateMachine stateMachine) =>
            _stateMachine = stateMachine;


        public void Enable()
        {
            _inventoryService.OnInventoryChanged += RefreshSlots;
            _view.OnBackRequested += HandleBack;
            _view.OnSwapRequested += HandleSwap;
            RefreshSlots();

            _view.gameObject.SetActive(true);
        }

        private void HandleSwap(int flom, int to)
        {
            _organizer.SwapSlots(flom, to);
            RefreshSlots();
        }

        private void RefreshSlots()
        {
            for (var i = 0; i < InventoryOrganizer.SLOT_COUNT; i++)
            {
                var id = _organizer.GetSlotItem(i);
                var item = id != null ? _itemRepository.GetItemById(id) : null;
                _view.SetSlot(i, item);
            }
        }

        public void Disable()
        {
            _inventoryService.OnInventoryChanged -= RefreshSlots;
            _view.OnBackRequested -= HandleBack;
            _view.OnSwapRequested -= HandleSwap;
            _view.gameObject.SetActive(false);
        }


        private void HandleBack()
        {
            //TODO: Implement stack in state machine
            _stateMachine.SwitchTo<ShopState>();
        }
    }
}