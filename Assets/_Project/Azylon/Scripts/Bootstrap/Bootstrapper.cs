using System;
using System.Collections.Generic;
using Azylon.Currency;
using Azylon.Inventory;
using Azylon.ItemRepository;
using Azylon.Purchasing;
using Azylon.SaveData;
using Azylon.UI;
using Azylon.UI.Popups;
using Azylon.UI.Presenters;
using Azylon.UI.UIStates;
using Azylon.UI.Views;
using UnityEngine;

namespace Azylon.Bootstrap
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