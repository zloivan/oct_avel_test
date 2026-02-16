using System.Collections.Generic;
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
    /// </summary>
    public class CharactersView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _characters;
        
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            
            if (_text == null)
            {
                Debug.LogError("[CharactersView] Text component not found!");
            }
        }

        private void Update()
        {
            if (_characters == null || _characters.Count == 0)
            {
                _text.text = "Characters: 0 | Avg value: 0";
                return;
            }

            var totalValue = 0f;
            var validCount = 0;

            foreach (var characterTransform in _characters)
            {
                if (characterTransform == null) 
                    continue;

                //Get component in update in foreach might be expencive
                var character = characterTransform.GetComponent<Character>();
                
                if (character != null)
                {
                    totalValue += character.Value;
                    validCount++;
                }
            }

            var avgValue = validCount > 0 ? totalValue / validCount : 0f;

            _text.text = $"Characters: {validCount} | Avg value: {avgValue:F2}";
        }
    }
}