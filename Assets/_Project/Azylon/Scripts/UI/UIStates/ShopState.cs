namespace Azylon.UI.UIStates
{
    public class ShopState : IUIState
    {
        private readonly ShopPresenter _presenter;

        public ShopState(ShopPresenter presenter) =>
            _presenter = presenter;

        public void Enter() =>
            _presenter.Enable();

        public void Exit() =>
            _presenter.Disable();
    }

    public class ShopPresenter
    {
        public void Enable()
        {
            UnityEngine.Debug.Log("[ShopPresenter] Shop UI Enabled");
        }

        public void Disable()
        {
            UnityEngine.Debug.Log("[ShopPresenter] Shop UI Disabled");
        }
    }
}