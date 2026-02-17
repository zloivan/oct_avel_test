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

    public class RewardPresenter
    {
        public void Enable()
        {
            UnityEngine.Debug.Log("[RewardPresenter] Reward UI Enabled");
        }

        public void Disable()
        {
            UnityEngine.Debug.Log("[RewardPresenter] Reward UI Disabled");
        }
    }
}