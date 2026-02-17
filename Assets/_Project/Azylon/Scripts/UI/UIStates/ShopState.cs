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
}