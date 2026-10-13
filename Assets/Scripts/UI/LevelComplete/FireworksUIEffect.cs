using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireworksUIEffect : MonoBehaviour
{
    private const float EaseOutBase = 1f;
    private const float CenterAnchor = 0.5f;
    private const string SparkObjectName = "Burst";

    [SerializeField] private RectTransform _spawnArea;
    [SerializeField] private Sprite[] _burstSprites;
    [SerializeField] private Color[] _burstColors;
    [SerializeField] private int _burstCount = 8;
    [SerializeField] private float _sparkSize = 100f;
    [SerializeField] private float _riseDuration = 0.5f;
    [SerializeField] private float _riseSpriteScale = 0.3f;
    [SerializeField] private float _minHeightFraction = 0.4f;
    [SerializeField] private float _maxHeightFraction = 0.8f;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _maxScale = 1.5f;
    [SerializeField] private float _easeOutPower = 3f;

    [ContextMenu("Test Play")]
    public void Play()
    {
        if (_burstSprites == null || _burstSprites.Length == 0)
            return;

        for (int i = 0; i < _burstCount; i++)
            StartCoroutine(FlyAndBurst(CreateSpark()));
    }

    private Image CreateSpark()
    {
        var sparkObject = new GameObject(SparkObjectName, typeof(RectTransform), typeof(Image));
        sparkObject.transform.SetParent(_spawnArea, false);

        var image = sparkObject.GetComponent<Image>();
        image.sprite = _burstSprites[Random.Range(0, _burstSprites.Length)];
        image.raycastTarget = false;

        if (_burstColors != null && _burstColors.Length > 0)
            image.color = _burstColors[Random.Range(0, _burstColors.Length)];

        RectTransform rect = image.rectTransform;
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(CenterAnchor, CenterAnchor);
        rect.sizeDelta = Vector2.one * _sparkSize;
        rect.localScale = Vector3.one * _riseSpriteScale;
        rect.anchoredPosition = RandomBottomPosition();

        return image;
    }

    private Vector2 RandomBottomPosition()
    {
        Vector2 size = _spawnArea.rect.size;

        return new Vector2(Random.Range(-size.x / 2f, size.x / 2f), -size.y / 2f);
    }

    private IEnumerator FlyAndBurst(Image image)
    {
        RectTransform rect = image.rectTransform;
        Vector2 size = _spawnArea.rect.size;

        Vector2 start = rect.anchoredPosition;
        float targetY = Random.Range(size.y * _minHeightFraction, size.y * _maxHeightFraction) - size.y / 2f;
        Vector2 end = new Vector2(start.x, targetY);

        yield return RunTimed(_riseDuration, progress =>
            rect.anchoredPosition = Vector2.Lerp(start, end, progress));

        rect.anchoredPosition = end;

        yield return AnimateBurst(image);
    }

    private IEnumerator AnimateBurst(Image image)
    {
        Color color = image.color;

        yield return RunTimed(_animationDuration, progress =>
        {
            image.rectTransform.localScale =
                Vector3.one * Mathf.Lerp(_riseSpriteScale, _maxScale, EaseOut(progress));

            color.a = Mathf.Lerp(1f, 0f, progress);
            image.color = color;
        });

        Destroy(image.gameObject);
    }

    private IEnumerator RunTimed(float duration, System.Action<float> onProgress)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            onProgress(elapsed / duration);
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }

    private float EaseOut(float progress)
    {
        return EaseOutBase - Mathf.Pow(EaseOutBase - progress, _easeOutPower);
    }
}