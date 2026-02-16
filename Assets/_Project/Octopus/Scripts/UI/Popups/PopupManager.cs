using JetBrains.Annotations;
using UnityEngine;

namespace Octopus.UI.Popups
{
    [UsedImplicitly]
    public class PopupManager
    {
        private readonly PopupViewUI _popupPrefab;
        private readonly Transform _uiRoot;
        private PopupViewUI _currentPopup;

        public PopupManager(PopupViewUI popupPrefab, Transform uiRoot)
        {
            _popupPrefab = popupPrefab;
            _uiRoot = uiRoot;
        }

        public void ShowPopup(PopupConfig config)
        {
            if (_popupPrefab == null || _uiRoot == null)
                return;
            
            if (_currentPopup != null)
            {
                HidePopup();
            }

            _currentPopup = Object.Instantiate(_popupPrefab, _uiRoot);
            _currentPopup.Show(config);
        }

        public void HidePopup()
        {
            if (_currentPopup == null) 
                return;
            
            _currentPopup.Hide();
            Object.Destroy(_currentPopup.gameObject);
            _currentPopup = null;
        }
    }
}