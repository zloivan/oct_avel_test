using Azulon.UI.Presenters;

namespace Azulon.UI.UIStates
{
    public class RewardState : IUIState
    {
        private readonly RewardPresenter _presenter;

        public RewardState(RewardPresenter presenter)
        {
            _presenter = presenter;
            _presenter.Disable();
        }

        public void Enter() =>
            _presenter.Enable();

        public void Exit() =>
            _presenter.Disable();
    }
}