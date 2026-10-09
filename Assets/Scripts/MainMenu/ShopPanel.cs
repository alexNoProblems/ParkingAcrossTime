using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ShopPanel : MonoBehaviour
{
   [SerializeField] private CoinWalletService _walletService;
   [SerializeField] private Button _rewardedButton;
   [SerializeField] private string _rewardedId = "shop_coins";
   [SerializeField] private int _rewardedCoins = 50;
   [SerializeField] private List<ShopProductView> _products;

   private bool _isWaitingReward;

   private void OnEnable()
   {
      if (_walletService == null)
         _walletService = FindFirstObjectByType<CoinWalletService>();
      
      YG2.onPurchaseSuccess += OnPurchaseSuccess;
      YG2.onRewardAdv += OnRewardAdv;
      YG2.onCloseRewardedAdv += OnRewardedClosed;
      YG2.onPurchaseFailed += OnPurchaseFailed;
      
      _rewardedButton.onClick.AddListener(OnRewardedClicked);

      foreach (ShopProductView product in _products)
      {
         ShopProductView captured = product;
         captured.Button.onClick.AddListener(() => YG2.BuyPayments(captured.ProductID));
         captured.RefreshPrice();
      }
   }

   private void OnDisable()
   {
      YG2.onPurchaseSuccess -= OnPurchaseSuccess;
      YG2.onRewardAdv -= OnRewardAdv;
      YG2.onCloseRewardedAdv -= OnRewardedClosed;
      YG2.onPurchaseFailed -= OnPurchaseFailed;
      
      _rewardedButton.onClick.RemoveListener(OnRewardedClicked);

      foreach (ShopProductView product in _products)
      {
         if (product != null && product.Button != null)
            product.Button.onClick.RemoveAllListeners();
      }
   }

   private void OnRewardedClicked()
   {
      if (_isWaitingReward)
         return;
      
      _isWaitingReward = true;
      _rewardedButton.interactable = false;
      YG2.RewardedAdvShow(_rewardedId);
   }
   
   private void OnRewardAdv(string id)
   {
      if (id != _rewardedId)
         return;
      
      _walletService.Wallet.Add(_rewardedCoins);
   }

   private void OnRewardedClosed()
   {
      _isWaitingReward = false;
      _rewardedButton.interactable = true;
   }

   private void OnPurchaseSuccess(string id)
   {
      foreach (ShopProductView product in _products)
      {
         if (product.ProductID == id)
         {
            _walletService.Wallet.Add(product.Coins);
            
            return;
         }
      }
   }

   private void OnPurchaseFailed(string id)
   {
      Debug.LogWarning($"Покупка не удалась: {id} ");
   }
}
