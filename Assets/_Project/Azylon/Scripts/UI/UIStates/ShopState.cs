using Azylon.UI.Presenters;

namespace Azylon.UI.UIStates
{
    public class ShopState : IUIState
    {
        private readonly ShopPresenter _presenter;

        public ShopState(ShopPresenter presenter)
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