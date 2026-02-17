using Azylon.ItemRepository;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Azylon.UI.Views
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameLabel;

        public void Setup(ItemDataSO item)
        {
            _icon.sprite = item.Icon;
            _nameLabel.text = item.ItemName;
        }
    }
}