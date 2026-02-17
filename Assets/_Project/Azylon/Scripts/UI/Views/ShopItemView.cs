using System;
using Azylon.ItemRepository;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Azylon.UI.Views
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameLabel;
        [SerializeField] private TextMeshProUGUI _priceLabel;
        [SerializeField] private TextMeshProUGUI _descriptionLabel;
        [SerializeField] private Button _buyButton;

        private ItemDataSO _item;
        private Action<ItemDataSO> _onPurchaseRequested;

        private void OnEnable() =>
            _buyButton.onClick.AddListener(HandleCallback);

        private void OnDisable() =>
            _buyButton.onClick.RemoveListener(HandleCallback);

        public void Setup(ItemDataSO item, Action<ItemDataSO> onPurchaseRequested)
        {
            _icon.sprite = item.Icon;
            _nameLabel.text = item.ItemName;
            _descriptionLabel.text = item.Description;
            _item = item;
            _priceLabel.text = $"Price: {item.Price}";
            _onPurchaseRequested = onPurchaseRequested;
        }

        private void HandleCallback() =>
            _onPurchaseRequested?.Invoke(_item);
    }
}