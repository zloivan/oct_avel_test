namespace Azylon.UI.UIStates
{
    public class InventoryState : IUIState
    {
        private readonly InventoryPresenter _presenter;

        public InventoryState(InventoryPresenter presenter) =>
            _presenter = presenter;

        public void Enter() =>
            _presenter.Enable();

        public void Exit() =>
            _presenter.Disable();
    }

    public class InventoryPresenter
    {
        public void Enable()
        {
            UnityEngine.Debug.Log("[InventoryPresenter] Inventory UI Enabled");
        }

        public void Disable()
        {
            UnityEngine.Debug.Log("[InventoryPresenter] Inventory UI Disabled");
        }
    }
}