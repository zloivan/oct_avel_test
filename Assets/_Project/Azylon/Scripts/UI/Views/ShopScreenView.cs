using System;
using System.Collections.Generic;
using Azylon.ItemRepository;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Azylon.UI.Views
{
    public class ShopScreenView : MonoBehaviour
    {
        public event Action<ItemDataSO> OnPurchaseRequested;
        public event Action OnInventoryRequested;
        public event Action OnEarnCurrencyRequested;

        [SerializeField] private TextMeshProUGUI _currencyLabel;
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private ShopItemView _itemViewPrefab;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _earnCurrencyButton;


        private void Awake()
        {
            _inventoryButton.onClick.AddListener(() => OnInventoryRequested?.Invoke());
            _earnCurrencyButton.onClick.AddListener(() => OnEarnCurrencyRequested?.Invoke());
        }

        private void OnDestroy()
        {
            _inventoryButton.onClick.RemoveAllListeners();
            _earnCurrencyButton.onClick.RemoveAllListeners();
        }

        public void SetItems(IReadOnlyList<ItemDataSO> items)
        {
            foreach (Transform child in _itemsContainer)
                Destroy(child.gameObject);

            foreach (var item in items)
            {
                var itemView = Instantiate(_itemViewPrefab, _itemsContainer);
                itemView.Setup(item, HandlePurchaseRequested);
            }
        }

        private void HandlePurchaseRequested(ItemDataSO selected)
        {
            OnPurchaseRequested?.Invoke(selected);
        }

        public void SetCurrency(int amount) =>
            _currencyLabel.text = $"Coins: {amount}";
    }
}