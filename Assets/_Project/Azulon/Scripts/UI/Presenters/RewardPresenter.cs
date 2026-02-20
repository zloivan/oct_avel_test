using Azulon.Currency;
using Azulon.UI.UIStates;
using Azulon.UI.Views;

namespace Azulon.UI.Presenters
{
    public class RewardPresenter
    {
        private readonly RewardScreenView _view;
        private UIStateMachine _stateMachine;
        private readonly ICurrencyService _currencyService;

        public RewardPresenter(RewardScreenView view, ICurrencyService currencyService)
        {
            _view = view;
            _currencyService = currencyService;
        }

        public void Inject(UIStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enable()
        {
            _view.OnRewardRequested += HandleRewardRequest;
            _view.StartTimingBar();
            _view.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _view.OnRewardRequested -= HandleRewardRequest;
            _view.gameObject.SetActive(false);
        }

        private void HandleRewardRequest(int rewardAmount)
        {
            _currencyService.Add(rewardAmount);
            _stateMachine.SwitchTo<ShopState>();
        }
    }
}