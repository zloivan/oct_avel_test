using UnityEngine;

namespace Azylon.Currency
{
    [CreateAssetMenu(fileName = "New Currency Config", menuName = "Azylon/Configs/Currency Config", order = 0)]
    public class CurrencyConfig : ScriptableObject
    {
        [SerializeField] private int _startingAmount = 100;
        
        public int GetStartingAmount() => _startingAmount;
    }
}