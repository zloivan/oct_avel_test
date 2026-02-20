using Azulon.UI.Presenters;

namespace Azulon.UI.UIStates
{
    public class InventoryState : IUIState
    {
        private readonly InventoryPresenter _presenter;

        public InventoryState(InventoryPresenter presenter)
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