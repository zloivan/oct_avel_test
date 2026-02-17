using System;
using System.Collections.Generic;
using Azylon.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

namespace Azylon.UI.UIStates
{
    public class ShopScreen : MonoBehaviour
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
                itemView.Setup(item, OnPurchaseRequested);
            }
        }

        public void SetCurrency(int amount) =>
            _currencyLabel.text = $"Coins: {amount}";
    }
}