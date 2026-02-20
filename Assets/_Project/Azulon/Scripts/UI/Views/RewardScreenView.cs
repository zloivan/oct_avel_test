using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Azulon.UI.Views
{
    public class RewardScreenView : MonoBehaviour
    {
        public event Action<int> OnRewardRequested;

        [SerializeField] private RectTransform _marker;
        [SerializeField] private RectTransform _greenZone;
        [SerializeField] private RectTransform _bar;
        [SerializeField] private Button _claimButton;
        [SerializeField] private TextMeshProUGUI _rewardPreviewLabel;
        [SerializeField] private int _fullReward = 100;
        [SerializeField] private int _partialReward = 25;
        [SerializeField] private float _markerSpeedModifier = 200f;

        private float _barWidth;
        private float _markerPosition;
        private bool _isRunning;
        private Tweener _markerTween;


        private void Awake()
        {
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

        [ContextMenu("Start Timing Bar")]
        public void StartTimingBar()
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

        private void OnDestroy()
        {
            _markerTween?.Kill();
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
            _isRunning = false;
            _markerTween?.Kill();
            OnRewardRequested?.Invoke(IsInGreenZone() ? _fullReward : _partialReward);
        }
    }
}