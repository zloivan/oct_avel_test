namespace Azylon.UI.UIStates
{
    public class RewardState : IUIState
    {
        private readonly RewardPresenter _presenter;

        public RewardState(RewardPresenter presenter) =>
            _presenter = presenter;

        public void Enter() =>
            _presenter.Enable();

        public void Exit() =>
            _presenter.Disable();
    }
}