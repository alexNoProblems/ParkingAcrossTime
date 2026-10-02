using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireworksUIEffect : MonoBehaviour
{
    [SerializeField] private RectTransform _spawnArea;
    [SerializeField] private Sprite[] _burstSprites;
    [SerializeField] private Color[] _burstColors;
    [SerializeField] private int _burstCount = 8;
    [SerializeField] private float _riseDuration = 0.5f;
    [SerializeField] private float _riseSpriteScale = 0.3f;
    [SerializeField] private float _minHeightFraction = 0.4f;
    [SerializeField] private float _maxHeightFraction = 0.8f;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _maxScale = 1.5f;
    [SerializeField] private float _easeOutBase = 1f;
    [SerializeField] private float _easeOutPower = 3f;

    [ContextMenu("Test Play")]
    public void Play()
    {
        StartCoroutine(SpawnBursts());
    }

    private IEnumerator SpawnBursts()
    {
        for (int i = 0; i < _burstCount; i++)
            StartCoroutine(FlyAndBurst(CreateSpark()));
        
        yield break;
    }

    private Image CreateSpark()
    {
        var sparkObject = new GameObject("Burst", typeof(RectTransform), typeof(Image));
        sparkObject.transform.SetParent(_spawnArea, false);

        var image = sparkObject.GetComponent<Image>();
        image.sprite = _burstSprites[Random.Range(0, _burstSprites.Length)];
        image.raycastTarget = false;

        if (_burstColors != null && _burstColors.Length > 0)
            image.color = _burstColors[Random.Range(0, _burstColors.Length)];

        RectTransform rect = image.rectTransform;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(100f, 100f);
        rect.localScale = Vector3.one * _riseSpriteScale;
        rect.anchoredPosition = RandomBottomPosition();

        return image;
    }

    private Vector2 RandomBottomPosition()
    {
        Vector2 size = _spawnArea.rect.size;

        float x = Random.Range(-size.x / 2f, size.x / 2f);
        float y = -size.y / 2f;

        return new Vector2(x, y);
    }

    private IEnumerator FlyAndBurst(Image image)
    {
        RectTransform rect = image.rectTransform;
        Vector2 size = _spawnArea.rect.size;

        Vector2 start = rect.anchoredPosition;
        float targetY = Random.Range(size.y * _minHeightFraction, size.y * _maxHeightFraction) - size.y / 2f;
        Vector2 end = new Vector2(start.x, targetY);
        
        Debug.Log($"spawnArea size={size}, start={start}, end={end}, riseDuration={_riseDuration}");

        float elapsed = 0f;

        while (elapsed < _riseDuration)
        {
            float progress = elapsed / _riseDuration;

            rect.anchoredPosition = Vector2.Lerp(start, end, progress);
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        rect.anchoredPosition = end;

        yield return AnimateBurst(image);
    }

    private IEnumerator AnimateBurst(Image image)
    {
        float elapsed = 0f;
        Color color = image.color;

        while (elapsed < _animationDuration)
        {
            float progress = elapsed / _animationDuration;

            image.rectTransform.localScale = Vector3.one * Mathf.Lerp(_riseSpriteScale, _maxScale, EaseOut(progress));
            color.a = Mathf.Lerp(1f, 0f, progress);
            image.color = color;

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        Destroy(image.gameObject);
    }

    private float EaseOut(float progress)
    {
        return _easeOutBase - Mathf.Pow(_easeOutBase - progress, _easeOutPower);
    }
}