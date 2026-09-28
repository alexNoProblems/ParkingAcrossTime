using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LevelRewardPanel : MonoBehaviour
{
    private const string RewardedAdId = "level_reward";

    [SerializeField] private LevelDataInitializer _levelData;
    [SerializeField] private CoinFlyAnimator _coinFly;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private Button _claimButton;
    [SerializeField] private Button _adButton;
    [SerializeField] private int _baseReward = 15;
    [SerializeField] private int _adMultiplier = 5;
    [SerializeField] private string _levelTextFormat = "Уровень {0} пройден!";
    
    private CoinWalletService _walletService;
    private bool _isClaimed;

    private void OnEnable()
    {
        _walletService = FindAnyObjectByType<CoinWalletService>();
        _isClaimed = false;
        
        _levelText.text = string.Format(_levelTextFormat, _levelData.CurrentLevel);
        _rewardText.text = $"+{_baseReward}";
        
        _claimButton.onClick.AddListener(OnClaimClicked);
        _adButton.onClick.AddListener(OnAdClicked);
        YG2.onRewardAdv += OnRewardAdv;
    }

    private void OnDisable()
    {
        _claimButton.onClick.RemoveListener(OnClaimClicked);
        _adButton.onClick.RemoveListener(OnAdClicked);
        YG2.onRewardAdv -= OnRewardAdv;
    }

    private void OnClaimClicked()
    {
        Claim(_baseReward);
    }

    private void OnAdClicked()
    {
        YG2.RewardedAdvShow(RewardedAdId);
    }

    private void OnRewardAdv(string id)
    {
        if (id != RewardedAdId)
            return;
        
        Claim(_baseReward * _adMultiplier);
    }

    private void Claim(int amount)
    {
        if (_isClaimed)
            return;

        _isClaimed = true;
        _claimButton.interactable = false;
        _adButton.interactable = false;

        _coinFly.Play(() =>
        {
            _walletService.Wallet.Add(amount);
            gameObject.SetActive(false);
        });
    }
}
