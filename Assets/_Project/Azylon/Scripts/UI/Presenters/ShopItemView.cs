using System;
using Azylon.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Azylon.UI.UIStates
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameLabel;
        [SerializeField] private TextMeshProUGUI _priceLabel;
        [SerializeField] private TextMeshProUGUI _descriptionLabel;
        [SerializeField] private Button _buyButton;


        public void Setup(ItemDataSO item, Action<ItemDataSO> onPurchaseRequested)
        {
            _icon.sprite = item.Icon;
            _nameLabel.text = item.ItemName;
            _descriptionLabel.text = item.Description;
 
            _priceLabel.text = $"Price: {item.Price}";

            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(() => onPurchaseRequested?.Invoke(item));
        }
    }
}