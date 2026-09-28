using System;
using TMPro;
using UnityEngine;

public class CoinWalletView : MonoBehaviour
{
    [SerializeField] private CoinWalletService _coinWalletService;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private RectTransform _flyTarget;
    
    public RectTransform FlyTarget => _flyTarget;

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
