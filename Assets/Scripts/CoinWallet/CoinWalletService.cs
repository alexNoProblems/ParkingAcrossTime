using UnityEngine;
using YG;

public class CoinWalletService : MonoBehaviour
{
    public CoinWallet Wallet { get; } = new CoinWallet();

    private void Awake()
    {
        var existing = FindObjectsByType<CoinWalletService>(FindObjectsSortMode.None);

        if (existing.Length > 1)
        {
            Destroy(gameObject);
         
            return;
        }
      
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += OnGetSDKData;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= OnGetSDKData;
    }

    private void OnGetSDKData()
    {
        Wallet.NotifyLoaded();
    }
}