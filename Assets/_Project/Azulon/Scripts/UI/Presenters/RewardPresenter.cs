using Azulon.Currency;
using Azulon.UI.Popups;
using Azulon.UI.UIStates;
using Azulon.UI.Views;
using Cysharp.Threading.Tasks;

namespace Azulon.UI.Presenters
{
    public class RewardPresenter
    {
        private readonly RewardScreenView _view;
        private readonly ICurrencyService _currencyService;
        private readonly PopupManager _popupManager;
        private UIStateMachine _stateMachine;

        public RewardPresenter(RewardScreenView view, ICurrencyService currencyService, PopupManager popupManager)
        {
            _view = view;
            _currencyService = currencyService;
            _popupManager = popupManager;
        }

        public void Inject(UIStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enable()
        {
            _view.OnRewardRequested += HandleRewardRequest;
            _view.gameObject.SetActive(true);
            _view.Activate();
        }

        public void Disable()
        {
            _view.OnRewardRequested -= HandleRewardRequest;
            _view.Deactivate();
            _view.gameObject.SetActive(false);
        }

        private void HandleRewardRequest(int rewardAmount, bool isHit)
        {
            _currencyService.Add(rewardAmount);
            _view.SpawnFloatingText(rewardAmount, isHit);
            ShowResultPopup(rewardAmount, isHit).Forget();
        }

        private async UniTaskVoid ShowResultPopup(int amount, bool isHit)
        {
            const int DELAY_BEFORE_POPUP_MS = 850;

            await UniTask.Delay(DELAY_BEFORE_POPUP_MS);

            var config = new PopupConfig(
                isHit ? "Perfect!" : "So close!",
                $"You earned {amount} coins!",
                new PopupButton("OK", () =>
                {
                    _popupManager.HidePopup();
                    _stateMachine.SwitchTo<ShopState>();
                })
            );

            _popupManager.ShowPopup(config);
        }
    }
}