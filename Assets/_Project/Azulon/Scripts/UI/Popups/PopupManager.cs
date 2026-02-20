using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Azulon.UI.Popups
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
                HidePopup();

            _currentPopup = Object.Instantiate(_popupPrefab, _uiRoot);
            _currentPopup.Show(config);
        }

        public void HidePopup()
        {
            if (_currentPopup == null)
                return;

            var popup = _currentPopup;
            
            _currentPopup = null;
            AwaitAndDestroy(popup).Forget();
        }
        
        private async UniTaskVoid AwaitAndDestroy(PopupViewUI popup)
        {
            await popup.HideAsync();
            if (popup != null)
                Object.Destroy(popup.gameObject);
        }
    }
}