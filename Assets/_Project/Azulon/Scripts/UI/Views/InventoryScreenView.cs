using System;
using Azulon.Inventory;
using Azulon.ItemRepository;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Azulon.UI.Views
{
    public class InventoryScreenView : MonoBehaviour
    {
        public event Action OnBackRequested;
        public event Action<int, int> OnSwapRequested;


        [FormerlySerializedAs("_itemsContainer")] [SerializeField]
        private Transform _slotsContainer;

        [FormerlySerializedAs("_itemViewPrefab")] [SerializeField]
        private InventorySlotView _slotViewPrefab;

        [SerializeField] private Button _backButton;

        private InventorySlotView[] _slotsArray;

        private void Awake()
        {
            foreach (Transform child in _slotsContainer)
                Destroy(child.gameObject);
            
            _backButton.onClick.AddListener(() => OnBackRequested?.Invoke());

            _slotsArray = new InventorySlotView[InventoryOrganizer.SLOT_COUNT];
            for (var i = 0; i < InventoryOrganizer.SLOT_COUNT; i++)
            {
                var slotView = Instantiate(_slotViewPrefab, _slotsContainer);
                slotView.SetIndex(i);
                _slotsArray[i] = slotView;
            }
        }

        private void OnDestroy() => _backButton.onClick.RemoveAllListeners();
        
        public void SetSlot(int index, ItemDataSO item) =>
            _slotsArray[index].SetItem(item);
    }
}