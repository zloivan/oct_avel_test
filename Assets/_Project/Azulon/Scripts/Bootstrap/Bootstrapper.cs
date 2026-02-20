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
            // Data
            var itemRepository = new ScriptableObjectItemRepository(_itemDataArray);
            var saveSystem = new SaveSystem();

            // Services
            var currencyService = new CurrencyService(_currencyConfigSO, saveSystem);
            var inventoryService = new InventoryService(saveSystem);
            var shopService = new ShopService(itemRepository, currencyService, inventoryService);

            // UI Infrastructure
            var popupManager = new PopupManager(_popupView, _uiRoot);

            // Presenters — без StateMachine
            var shopPresenter = new ShopPresenter(_shopScreenView, shopService, currencyService, popupManager);
            var inventoryPresenter = new InventoryPresenter(_inventoryScreenView, inventoryService, itemRepository);
            var rewardPresenter = new RewardPresenter(_rewardScreenView, currencyService);

            // FSM
            var stateMachine = new UIStateMachine(new IUIState[]
            {
                new ShopState(shopPresenter),
                new InventoryState(inventoryPresenter),
                new RewardState(rewardPresenter)
            });

            // Inject FSM (circular dep resolution)
            shopPresenter.Inject(stateMachine);
            inventoryPresenter.Inject(stateMachine);
            rewardPresenter.Inject(stateMachine);

            // Start
            stateMachine.SwitchTo<ShopState>();
        }
    }
}