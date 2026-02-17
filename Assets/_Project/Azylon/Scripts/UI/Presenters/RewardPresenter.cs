using Azylon.Currency;
using Azylon.UI.UIStates;
using Azylon.UI.Views;

namespace Azylon.UI.Presenters
{
    public class RewardPresenter
    {
        private readonly RewardScreenView _view;
        private readonly UIStateMachine _stateMachine;
        private readonly ICurrencyService _currencyService;
        
        
        public RewardPresenter(RewardScreenView view, ICurrencyService currencyService, UIStateMachine stateMachine)
        {
            _view = view;
            _currencyService = currencyService;
            _stateMachine = stateMachine;
        }


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
            _stateMachine.SwitchTo<InventoryState>();
        }
    }
}