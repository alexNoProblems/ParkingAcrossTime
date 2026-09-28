using System;
using TMPro;
using UnityEngine;

public class CoinWalleView : MonoBehaviour
{
    [SerializeField] private CoinWalletService _coinWalletService;
    [SerializeField] private TextMeshProUGUI _coinText;

    private void OnEnable()
    {
        _coinWalletService.Wallet.BalanceChanged += UpdateText;
        UpdateText(_coinWalletService.Wallet.Balance);
    }

    private void OnDisable()
    {
        _coinWalletService.Wallet.BalanceChanged -= UpdateText;
    }

    private void UpdateText(int balance)
    {
        _coinText.text = balance.ToString();
    }
}
