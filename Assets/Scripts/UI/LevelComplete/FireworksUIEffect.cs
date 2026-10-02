
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireworksUIEffect : MonoBehaviour
{
    [SerializeField] private RectTransform _spawnArea;
    [SerializeField] private Sprite[] _burstSprites;
    [SerializeField] private int _burstCount = 8;
    [SerializeField] private float _burstInterval = 0.2f;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _maxScale = 1.5f;
    [SerializeField] private float _easeOutBase = 1f;
    [SerializeField] private float _easeOutPower = 3f;

    private WaitForSeconds _burstWait;

    private void Awake()
    {
        _burstWait = new WaitForSeconds(_burstInterval);
    }
    
    public void Play()
    {
        StartCoroutine(SpawnBursts());
    }

    private IEnumerator SpawnBursts()
    {
        for (int i = 0; i < _burstCount; i++)
        {
            StartCoroutine(AnimateBurst(CreateBurst()));

            yield return _burstWait;
        }
    }

    private Image CreateBurst()
    {
        var burstObject = new GameObject("Burst", typeof(RectTransform), typeof(Image));
        burstObject.transform.SetParent(_spawnArea, false);

        var image = burstObject.GetComponent<Image>();
        image.sprite = _burstSprites[Random.Range(0, _burstSprites.Length)];
        image.raycastTarget = false;

        RectTransform rect = image.rectTransform;
        rect.sizeDelta = new Vector2(100f, 100f);
        rect.anchoredPosition = RandomPositionInArea();
        rect.localScale = Vector3.zero;

        return image;
    }

    private Vector2 RandomPositionInArea()
    {
        Vector2 size = _spawnArea.rect.size;

        float x = Random.Range(-size.x / 2f, size.x / 2f);
        float y = Random.Range(-size.y / 2f, size.y / 2f);

        return new Vector2(x, y);
    }

    private IEnumerator AnimateBurst(Image image)
    {
        float elapsed = 0f;
        Color color = image.color;

        while (elapsed < _animationDuration)
        {
            float progress = elapsed / _animationDuration;

            image.rectTransform.localScale = Vector3.one * Mathf.Lerp(0f, _maxScale, EaseOut(progress));
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