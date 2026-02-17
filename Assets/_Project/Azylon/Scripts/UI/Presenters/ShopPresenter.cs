using Azylon.Currency;
using Azylon.ItemRepository;
using Azylon.Purchasing;
using Azylon.UI.Popups;
using Azylon.UI.UIStates;
using Azylon.UI.Views;

namespace Azylon.UI.Presenters
{
    public class ShopPresenter
    {
        private readonly ShopScreenView _view;
        private readonly IShopService _shopService;
        private readonly ICurrencyService _currencyService;
        private readonly PopupManager _popupManager;

        private UIStateMachine _stateMachine;

        public ShopPresenter(ShopScreenView view, IShopService shopService, ICurrencyService currencyService,
             PopupManager popupManager)
        {
            _view = view;
            _shopService = shopService;
            _currencyService = currencyService;
            _popupManager = popupManager;
        }

        public void Inject(UIStateMachine stateMachine) 
            => _stateMachine = stateMachine;

        public void Enable()
        {
            _view.SetItems(_shopService.GetAvailableItemsList());
            _view.SetCurrency(_currencyService.GetAmount());
            _currencyService.OnAmountChanged += HandleCurrencyAmountChanged;
            _view.OnPurchaseRequested += HandlePurchaseRequest;
            _view.OnInventoryRequested += HandleInventoryRequest;
            _view.OnEarnCurrencyRequested += HandleEarnCurrencySwitch;
            _view.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _currencyService.OnAmountChanged -= HandleCurrencyAmountChanged;
            _view.OnPurchaseRequested -= HandlePurchaseRequest;
            _view.OnInventoryRequested -= HandleInventoryRequest;
            _view.OnEarnCurrencyRequested -= HandleEarnCurrencySwitch;
            _view.gameObject.SetActive(false);
        }

        private void HandleCurrencyAmountChanged(int newCurrencyAmount) =>
            _view.SetCurrency(newCurrencyAmount);


        private void HandlePurchaseRequest(ItemDataSO itemId)
        {
            var result = _shopService.TryPurchaseItem(itemId);
            switch (result)
            {
                case PurchaseResult.Success:
                    _popupManager.ShowPopup(new PopupConfig("Purchase Successful",
                        "You have successfully purchased the item.", new PopupButton("OK", () => { })));
                    break;

                case PurchaseResult.ItemAlreadyOwned:
                    _popupManager.ShowPopup(new PopupConfig("Purchase Failed", "You already own this item.",
                        new PopupButton("OK", () => { })));
                    break;

                case PurchaseResult.InsufficientFunds:
                    _popupManager.ShowPopup(new PopupConfig("Purchase Failed",
                        "You do not have enough currency to purchase this item.", new PopupButton("OK", () => { })));
                    break;
            }
        }

        private void HandleInventoryRequest() =>
            _stateMachine.SwitchTo<InventoryState>();

        private void HandleEarnCurrencySwitch() =>
            _stateMachine.SwitchTo<RewardState>();
    }
}