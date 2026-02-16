using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Octopus.Scripts.Gameplay
{
    /// <summary>
    /// Displays live stats about active gameplay characters.
    /// BUGFIXES APPLIED:
    /// * Fixed GetComponents → GetComponent (was returning array, now single component)
    /// * Fixed division order (was dividing count by total, now total by count)
    /// * Moved from FixedUpdate to Update (UI updates don't need physics timing)
    /// * Cached Text component (was calling GetComponent every frame)
    ///  /// Optimized version with:
    /// - Cached Character components (no GetComponent per frame)
    /// - Update throttling (configurable interval)
    /// - StringBuilder for text formatting (reduced GC)
    /// </summary>
    public class CharactersView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _characters;
        [SerializeField] private float _updateInterval = 0.2f; // Update every 200ms

        private Text _text;
        private readonly List<Character> _cachedCharacters = new();
        private readonly StringBuilder _stringBuilder = new();
        private float _timeSinceLastUpdate;

        private void Awake()
        {
            _text = GetComponent<Text>();

            if (_text == null)
            {
                Debug.LogError("[CharactersView] Text component not found!");
            }

            CacheCharacterComponents();
        }

        private void Update()
        {
            _timeSinceLastUpdate += Time.deltaTime;

            if (_timeSinceLastUpdate >= _updateInterval)
            {
                UpdateDisplay();
                _timeSinceLastUpdate = 0f;
            }
        }

        private void CacheCharacterComponents()
        {
            _cachedCharacters.Clear();

            if (_characters == null) return;

            foreach (var t in _characters)
            {
                if (t == null) continue;

                var character = t.GetComponent<Character>();
                if (character != null)
                {
                    _cachedCharacters.Add(character);
                }
            }
        }

        private void UpdateDisplay()
        {
            if (_cachedCharacters.Count == 0)
            {
                _text.text = "Characters: 0 | Avg value: 0";
                return;
            }

            var totalValue = 0f;

            foreach (var character in _cachedCharacters)
            {
                if (character != null) // Handle destroyed characters
                {
                    totalValue += character.Value;
                }
            }

            var avgValue = _cachedCharacters.Count > 0 ? totalValue / _cachedCharacters.Count : 0f;

            // Use StringBuilder to reduce GC allocations
            _stringBuilder.Clear();
            _stringBuilder.Append("Characters: ");
            _stringBuilder.Append(_cachedCharacters.Count);
            _stringBuilder.Append(" | Avg value: ");
            _stringBuilder.Append(avgValue.ToString("F2"));

            _text.text = _stringBuilder.ToString();
        }

        [ContextMenu("Refresh Character Cache")]
        public void RefreshCharacterCache()
        {
            CacheCharacterComponents();
        }
    }
}