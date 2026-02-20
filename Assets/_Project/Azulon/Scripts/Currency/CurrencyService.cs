using System;
using Azulon.SaveData;

namespace Azulon.Currency
{
    public class CurrencyService : ICurrencyService
    {
        public event Action<int> OnAmountChanged;

        const string SAVE_KEY = "CurrencyAmount";

        private int _amount;
        private readonly ISaveSystem _saveSystem;

        public CurrencyService(CurrencyConfigSO configSO, ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _amount = _saveSystem.SaveExists(SAVE_KEY)
                ? _saveSystem.Load<CurrencyData>(SAVE_KEY).Amount
                : configSO.GetStartingAmount();
        }

        public int GetAmount() =>
            _amount;

        public void Add(int amount)
        {
            _amount += amount;
            _saveSystem.Save(SAVE_KEY, new CurrencyData { Amount = _amount });
            OnAmountChanged?.Invoke(_amount);
        }

        public bool TrySpend(int spendAmount)
        {
            if (_amount < spendAmount)
                return false;

            _amount -= spendAmount;
            _saveSystem.Save(SAVE_KEY, new CurrencyData { Amount = _amount });
            OnAmountChanged?.Invoke(_amount);
            return true;
        }
    }
}