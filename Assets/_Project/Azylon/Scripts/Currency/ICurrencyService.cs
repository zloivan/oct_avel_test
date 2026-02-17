using System;

namespace Azylon.Currency
{
    public interface ICurrencyService
    {
        event Action<int> OnAmountChanged;
        
        int GetAmount();
        void Add(int amount);
        bool TrySpend(int spendAmount);
    }
}