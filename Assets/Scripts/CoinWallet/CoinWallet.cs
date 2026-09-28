using System;
using YG;

public class CoinWallet
{
    public event Action<int> BalanceChanged;

    public int Balance => YG2.saves.coins;

    public void Add(int amount)
    {
        YG2.saves.coins += amount;
        YG2.SaveProgress();
        
        BalanceChanged?.Invoke(Balance);
    }

    public bool TrySpend(int amount)
    {
        if (YG2.saves.coins < amount)
            return false;
        
        YG2.saves.coins -= amount;
        YG2.SaveProgress();
        
        BalanceChanged?.Invoke(Balance);
        
        return true;
    }

    public void NotifyLoaded()
    {
        BalanceChanged?.Invoke(Balance);
    }
}
