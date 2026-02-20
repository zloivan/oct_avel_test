using System;
using Azulon.ItemRepository;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Azulon.UI.Views
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameLabel;
        [SerializeField] private GameObject _filledRoot;
        [SerializeField] private GameObject _emptyRoot;

        private CanvasGroup _canvasGroup;

        private int _index;
        private bool _isFilled;

        private void Awake() =>
            _canvasGroup = GetComponent<CanvasGroup>();

        public void SetItem(ItemDataSO item)
        {
            _isFilled = item != null;
            _filledRoot.SetActive(_isFilled);
            _emptyRoot.SetActive(!_isFilled);

            if (!_isFilled)
                return;

            _icon.sprite = item.Icon;
            _nameLabel.text = item.ItemName;
        }

        public void SetIndex(int index) =>
            _index = index;

        public int GetIndex() =>
            _index;

        public bool GetIsFilled() =>
            _isFilled;

        public void SetDimmed(bool dimmed)
        {
            if (_canvasGroup != null)
                _canvasGroup.alpha = dimmed ? 0.35f : 1f;
        }
    }
}