using Azulon.Currency;
using Azulon.Inventory;
using Azulon.ItemRepository;
using Azulon.Purchasing;
using Azulon.SaveData;
using Azulon.UI;
using Azulon.UI.Popups;
using Azulon.UI.Presenters;
using Azulon.UI.UIStates;
using Azulon.UI.Views;
using UnityEngine;

namespace Azulon.Bootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private ItemDataSO[] _itemDataArray;
        [SerializeField] private CurrencyConfigSO _currencyConfigSO;
        [SerializeField] private ShopScreenView _shopScreenView;
        [SerializeField] private InventoryScreenView _inventoryScreenView;
        [SerializeField] private RewardScreenView _rewardScreenView;
        [SerializeField] private PopupViewUI _popupView;
        [SerializeField] private Transform _uiRoot;


        private void Awake()
        {
            var itemRepository = new ScriptableObjectItemRepository(_itemDataArray);
            var saveSystem = new SaveSystem();

            var inventoryOrganizer = new InventoryOrganizer(saveSystem);

            var currencyService = new CurrencyService(_currencyConfigSO, saveSystem);
            var inventoryService = new InventoryService(inventoryOrganizer);
            var shopService = new ShopService(itemRepository, currencyService, inventoryService);

            var popupManager = new PopupManager(_popupView, _uiRoot);

            var shopPresenter = new ShopPresenter(_shopScreenView, shopService, currencyService, popupManager);
            var inventoryPresenter = new InventoryPresenter(_inventoryScreenView, inventoryService, itemRepository,
                inventoryOrganizer);

            var rewardPresenter = new RewardPresenter(_rewardScreenView, currencyService, popupManager);

            var stateMachine = new UIStateMachine(new IUIState[]
            {
                new ShopState(shopPresenter),
                new InventoryState(inventoryPresenter),
                new RewardState(rewardPresenter)
            });

            shopPresenter.Inject(stateMachine);
            inventoryPresenter.Inject(stateMachine);
            rewardPresenter.Inject(stateMachine);

            stateMachine.SwitchTo<ShopState>();
        }
    }
}