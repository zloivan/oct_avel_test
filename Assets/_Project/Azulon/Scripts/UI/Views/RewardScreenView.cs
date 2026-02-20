using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Azulon.UI.Views
{
    public class RewardScreenView : MonoBehaviour
    {
        public event Action<int, bool> OnRewardRequested;

        [SerializeField] private RectTransform _marker;
        [SerializeField] private RectTransform _greenZone;
        [SerializeField] private RectTransform _bar;
        [SerializeField] private Image _greenZoneImage;


        [SerializeField] private Button _claimButton;


        [SerializeField] private TextMeshProUGUI _rewardPreviewLabel;
        [SerializeField] private TextMeshProUGUI _floatingTextPrefab;
        [SerializeField] private RectTransform _feedbackSpawnPoint;


        [SerializeField] private int _fullReward = 100;
        [SerializeField] private int _partialReward = 25;
        [SerializeField] private float _markerSpeedModifier = 200f;

        private readonly Color _hitColor = new Color(0.2f, 0.85f, 0.25f);
        private readonly Color _missColor = new Color(0.9f, 0.2f, 0.2f);

        private Color _defaultZoneColor;
        private float _barWidth;
        private float _markerPosition;
        private bool _isRunning;
        private Tweener _markerTween;


        private void Awake()
        {
            _defaultZoneColor = _greenZoneImage.color;

            _claimButton.onClick.RemoveAllListeners();
            _claimButton.onClick.AddListener(OnClaimPressed);
        }

        private void Update()
        {
            if (!_isRunning)
            {
                return;
            }

            // Применить позицию к маркеру
            _marker.anchoredPosition = new Vector2(_markerPosition, _marker.anchoredPosition.y);

            // Обновить предпросмотр награды
            UpdateRewardPreview();
        }

        private void OnDestroy() =>
            _markerTween?.Kill();

        [ContextMenu("Start Timing Bar")]
        public void Activate()
        {
            _isRunning = true;
            _barWidth = _bar.rect.width;
            _markerPosition = -_barWidth / 2f;

            _markerTween?.Kill();

            var duration = _barWidth / _markerSpeedModifier;

            _markerTween = DOVirtual.Float(
                    -_barWidth / 2f,
                    _barWidth / 2f,
                    duration,
                    value => _markerPosition = value
                )
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void SpawnFloatingText(int amount, bool isHit)
        {
            if (_floatingTextPrefab == null || _feedbackSpawnPoint == null) return;
            AnimateFloatingText(amount, isHit ? _hitColor : _missColor).Forget();
        }

        private async UniTaskVoid PlayFeedback(bool isHit)
        {
            var color = isHit ? _hitColor : _missColor;

            _claimButton.transform
                .DOPunchScale(Vector3.one * 0.28f, 0.35f, 7, 0.4f);

            await _greenZoneImage.DOColor(color, 0.08f).AsyncWaitForCompletion();
            await _greenZoneImage.DOColor(_defaultZoneColor, 0.35f).AsyncWaitForCompletion();
        }

        private async UniTaskVoid AnimateFloatingText(int amount, Color color)
        {
            var txt = Instantiate(_floatingTextPrefab, transform);
            txt.text = $"+{amount}";
            txt.color = color;
            txt.rectTransform.anchoredPosition = _feedbackSpawnPoint.anchoredPosition;

            var targetY = txt.rectTransform.anchoredPosition.y + 90f;

            var moveTween = txt.rectTransform.DOAnchorPosY(targetY, 0.9f).SetEase(Ease.OutCubic);
            var fadeTween = txt.DOFade(0f, 0.15f).SetDelay(0.15f);

            await fadeTween.AsyncWaitForCompletion();

            moveTween.Kill();
            fadeTween.Kill();

            Destroy(txt.gameObject);
        }

        private void UpdateRewardPreview()
        {
            var isInGreenZone = IsInGreenZone();

            var currentReward = isInGreenZone ? _fullReward : _partialReward;
            _rewardPreviewLabel.text = $"{currentReward}";
        }

        private bool IsInGreenZone()
        {
            var greenZoneLeft = _greenZone.anchoredPosition.x - _greenZone.rect.width / 2f;
            var greenZoneRight = _greenZone.anchoredPosition.x + _greenZone.rect.width / 2f;

            return _markerPosition >= greenZoneLeft && _markerPosition <= greenZoneRight;
        }

        private void OnClaimPressed()
        {
            Deactivate();
            var isHit = IsInGreenZone();
            var amount = isHit ? _fullReward : _partialReward;

            PlayFeedback(isHit).Forget();
            OnRewardRequested?.Invoke(amount, isHit);
        }

        public void Deactivate()
        {
            _isRunning = false;
            _markerTween?.Kill();
        }
    }
}