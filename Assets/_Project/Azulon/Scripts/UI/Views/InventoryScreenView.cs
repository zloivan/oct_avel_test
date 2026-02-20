using System;
using System.Collections.Generic;
using Azulon.ItemRepository;
using UnityEngine;
using UnityEngine.UI;

namespace Azulon.UI.Views
{
    public class InventoryScreenView : MonoBehaviour
    {
        public event Action OnBackRequested;

        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private InventoryItemView _itemViewPrefab;
        [SerializeField] private Button _backButton;

        private void Awake()
        {
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => OnBackRequested?.Invoke());
        }

        public void SetItems(List<ItemDataSO> ownedItems)
        {
            foreach (Transform itemObjectss in _itemsContainer)
            {
                Destroy(itemObjectss.gameObject);
            }
            
            foreach (var item in ownedItems)
            {
                var itemView = Instantiate(_itemViewPrefab, _itemsContainer);
                itemView.Setup(item);
            }
        }
    }
}