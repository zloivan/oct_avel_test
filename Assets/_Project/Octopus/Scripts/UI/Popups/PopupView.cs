using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Octopus.UI.Popups
{
    public class PopupViewUI : MonoBehaviour, IPopup
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _bodyText;
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private Button _buttonPrefab;

        private readonly List<Button> _activeButtons = new();

        public void Show(PopupConfig config)
        {
            _titleText.text = config.Title;
            _bodyText.text = config.Body;

            ClearButtons();
            CreateButtons(config.Buttons);

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void CreateButtons(PopupButton[] buttons)
        {
            if (buttons == null || buttons.Length == 0 || buttons.Length > 5)
            {
                Debug.LogError("[PopupView] Invalid button count. Must be 1-5.");
                return;
            }

            foreach (var buttonData in buttons)
            {
                var button = Instantiate(_buttonPrefab, _buttonsContainer);
                button.GetComponentInChildren<TextMeshProUGUI>().text = buttonData.Text;
                
                button.onClick.AddListener(() =>
                {
                    buttonData.Callback?.Invoke();
                    Hide();
                });

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