using System;
using System.Collections;
using UnityEngine;

public class CoinFlyAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform _coinIcon;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _coinSound;
    [SerializeField] private int _coinCount = 8;
    [SerializeField] private float _spawnInterval = 0.1f;
    [SerializeField] private float _flightDuration = 0.6f;
    
    private WaitForSeconds _spawnWait;
    private RectTransform _target;
    private Action _onComplete;
    private int _arrivedCount;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_spawnInterval);
    }

    public void Play(Action onComplete)
    {
        _onComplete = onComplete;
        _arrivedCount = 0;
        _target = FindAnyObjectByType<CoinWalletView>().FlyTarget;

        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        for (int i = 0; i < _coinCount; i++)
        {
            if (_coinSound != null)
                _audioSource.PlayOneShot(_coinSound);
            
            StartCoroutine(FlyCoin(CreateCoin()));
            
            yield return _spawnWait;
        }
    }

    private IEnumerator FlyCoin(RectTransform coin)
    {
        Vector3 start = coin.position;
        float elapsed = 0f;

        while (elapsed < _flightDuration)
        {
            float progress = elapsed / _flightDuration;

            coin.position = Vector3.Lerp(start, _target.position, progress * progress);
            elapsed += Time.deltaTime;

            yield return null;
        }

        Destroy(coin.gameObject);
        _arrivedCount++;

        if (_arrivedCount == _coinCount)
            _onComplete?.Invoke();
    }

    private RectTransform CreateCoin()
    {
        RectTransform coin = Instantiate(_coinIcon, transform);

        coin.anchorMin = new Vector2(0.5f, 0.5f);
        coin.anchorMax = new Vector2(0.5f, 0.5f);
        coin.pivot = new Vector2(0.5f, 0.5f);
        coin.localScale = Vector3.one;
        coin.sizeDelta = _coinIcon.rect.size;
        coin.position = _coinIcon.position;

        return coin;
    }
}
