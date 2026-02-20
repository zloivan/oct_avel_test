using System;

namespace Azulon.Currency
{
    public interface ICurrencyService
    {
        event Action<int> OnAmountChanged;
        
        int GetAmount();
        void Add(int amount);
        bool TrySpend(int spendAmount);
    }
}