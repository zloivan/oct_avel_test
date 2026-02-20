using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Azulon.UI.Popups
{
    public class PopupViewUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _bodyText;
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _popupBody;


        private const float SHOW_DURATION = 0.25f;
        private const float HIDE_DURATION = 0.2f;

        private readonly List<Button> _activeButtons = new();

        public void Show(PopupConfig config)
        {
            SetContent(config);
            AnimateShow().Forget();
        }

        public async UniTask HideAsync()
        {
            await DOTween.Sequence().Join(_popupBody.DOScale(0f, HIDE_DURATION).SetEase(Ease.InBack))
                .Join(_canvasGroup.DOFade(0f, HIDE_DURATION))
                .AsyncWaitForCompletion();

            gameObject.SetActive(false);
        }

        private void SetContent(PopupConfig config)
        {
            _titleText.text = config.Title;
            _bodyText.text = config.Body;

            ClearButtons();
            CreateButtons(config.Buttons);
        }

        private async UniTaskVoid AnimateShow()
        {
            gameObject.SetActive(true);
            _popupBody.localScale = Vector3.zero;
            _canvasGroup.alpha = 0f;

            await DOTween.Sequence().Join(_popupBody.DOScale(1f, SHOW_DURATION).SetEase(Ease.OutBack))
                .Join(_canvasGroup.DOFade(1f, SHOW_DURATION))
                .AsyncWaitForCompletion();
        }

        private void CreateButtons(PopupButton[] buttons)
        {
            const int MAX_BUTTONS_COUNT = 5;

            if (buttons == null || buttons.Length == 0 || buttons.Length > MAX_BUTTONS_COUNT)
            {
                Debug.LogError("[PopupView] Invalid button count. Must be 1-5.");
                return;
            }

            foreach (var buttonData in buttons)
            {
                var button = Instantiate(_buttonPrefab, _buttonsContainer);
                button.GetComponentInChildren<TextMeshProUGUI>().text = buttonData.Text;

                button.onClick.AddListener(() => { buttonData.Callback?.Invoke(); });

                _activeButtons.Add(button);
            }
        }

        private void ClearButtons()
        {
            foreach (var button in _activeButtons)
            {
                Destroy(button.gameObject);
            }

            _activeButtons.Clear();
        }
    }
}